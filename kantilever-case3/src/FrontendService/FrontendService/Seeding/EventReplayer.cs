using System;
using System.Threading;
using FrontendService.Agents.Abstractions;
using FrontendService.Commands;
using FrontendService.Seeding.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Host;
using RabbitMQ.Client;

namespace FrontendService.Seeding
{
    public class EventReplayer : IEventReplayer
    {
        internal const int TimeOut = 10000;

        /// <summary>
        /// Unfortunately, replaying is not as straightforward as we'd like it to be with the current AuditLogger in place
        ///
        /// The HTTP command sent to the auditlogger returns a value that indicates how many events will be replayed,
        /// however, this command will also trigger the replay which means that events will stream in as soon as we
        /// know how many events are coming.
        ///
        /// Due to this, we have to start listening for events before we trigger the replay, which poses the issue:
        /// It is not possible to change the value in the 'counter' callback because it is contained in a closure.
        ///
        /// To get around this we've decided to *shudders* use a static variable that gets reset at the start
        /// of a replay trigger, this value can be mutated outside of the closure.
        ///
        /// Given that no 2 replays will happen at once, this is a somewhat acceptable solution.
        ///
        /// The Miffy library should probably be extended to contain a .QueueSetup function to start setting up queues
        /// without handling (read: 'counting') them.
        /// </summary>
        private static long _amountToBeReplayed = long.MaxValue;

        private readonly ILoggerFactory _loggerFactory;
        private readonly IServiceCollection _serviceCollection;
        private readonly IAuditAgent _auditAgent;
        private readonly ILogger<EventReplayer> _logger;

        public EventReplayer(IServiceCollection serviceCollection, IAuditAgent auditAgent, ILoggerFactory loggerFactory)
        {
            _serviceCollection = serviceCollection;
            _loggerFactory = loggerFactory;
            _auditAgent = auditAgent;
            _logger = loggerFactory.CreateLogger<EventReplayer>();
        }

        ///<inheritdoc/>
        public event StartedReplayingEventHandler StartedReplaying;

        /// <summary>
        /// Replay specific events with a certain topic, type and timestamp
        /// </summary>
        /// <param name="context">Context to use to connect to amqp</param>
        /// <param name="topic">Topic to replay</param>
        /// <param name="type">Type of events to replay</param>
        /// <param name="until">Timestamp at which to stop replaying</param>
        /// <exception cref="TimeoutException">Thrown when it takes to long for all events to come in</exception>
        public void ReplayEvents(IBusContext<IConnection> context, string topic, Type type, DateTime until)
        {
            _logger.LogInformation($"Initiating replay for exchange {context.ExchangeName} topic {topic}, type {type} and until date {until}");

            _logger.LogTrace($"Resetting variable {nameof(_amountToBeReplayed)} to {long.MaxValue}");
            _amountToBeReplayed = long.MaxValue;

            _logger.LogTrace("Setting up host builder");
            MicroserviceHostBuilder builder = new MicroserviceHostBuilder()
                .SetLoggerFactory(_loggerFactory)
                .RegisterDependencies(_serviceCollection)
                .WithBusContext(context)
                .UseConventions();

            _logger.LogTrace("Creating host");
            using IMicroserviceHost host = builder.CreateHost();
            host.Start();

            _logger.LogDebug($"Creating ReplayEventsCommand with event type {type}, exchange {context.ExchangeName}, topic {topic}, timestamp {until}");
            ReplayEventsCommand replayEventsCommand = new ReplayEventsCommand
            {
                EventType = type.Name,
                ExchangeName = context.ExchangeName,
                TopicFilter = topic,
                ToTimestamp = until.Ticks
            };

            _logger.LogTrace("Setting up reset event, amount to be replayed and amount replayed");
            ManualResetEvent resetEvent = new ManualResetEvent(false);
            long amountReplayed = 0;

            _logger.LogTrace("Adding listener to EventMessageReceived callback on host");
            host.EventMessageHandled += (message, args) =>
            {
                _logger.LogDebug($"Received message on replay host, message with topic {message.Topic}, type {message.EventType} and id {message.CorrelationId}, " +
                                 $"progress: {amountReplayed + 1}/{_amountToBeReplayed}");

                Interlocked.Increment(ref amountReplayed);

                if (amountReplayed >= _amountToBeReplayed)
                {
                    resetEvent.Set();
                }
            };

            _logger.LogDebug("Sending ReplayEventsAsync command");
            _amountToBeReplayed = long.Parse(_auditAgent.ReplayEventsAsync(replayEventsCommand).Result);

            if (_amountToBeReplayed <= 0)
            {
                _logger.LogInformation("No events need to be replayed, resuming main thread");
                return;
            }

            OnStartedReplaying();
            bool result = resetEvent.WaitOne(TimeOut);

            if (!result)
            {
                throw new TimeoutException($"Replaying {amountReplayed}/{_amountToBeReplayed} events took longer than {TimeOut}ms");
            }

            _logger.LogInformation($"Received all {type.Name} events");
        }

        protected virtual void OnStartedReplaying()
        {
            StartedReplaying?.Invoke();
        }
    }
}
