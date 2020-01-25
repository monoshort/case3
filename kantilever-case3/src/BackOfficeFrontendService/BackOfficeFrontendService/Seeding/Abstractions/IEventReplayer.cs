using System;
using Minor.Miffy;
using RabbitMQ.Client;

namespace BackOfficeFrontendService.Seeding.Abstractions
{
    /// <summary>
    /// Special component used to replay events over a bus
    /// </summary>
    public interface IEventReplayer
    {
        /// <summary>
        /// Event handler called when the replayer starts replaying
        /// </summary>
        event StartedReplayingEventHandler StartedReplaying;

        void ReplayEvents(IBusContext<IConnection> context, string topic, Type type, DateTime until);
    }

    /// <summary>
    /// Event handler that is called to indicate that we started replaying in the main thread
    /// </summary>
    public delegate void StartedReplayingEventHandler();
}
