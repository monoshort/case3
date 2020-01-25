using System;
using Minor.Miffy;
using RabbitMQ.Client;

namespace BestelService.Seeding.Abstractions
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
    /// We admit, this event handler seems a bit out of place and yes, it is
    ///
    /// But in order to properly component test this without facing (a massive amount of) race conditions
    /// we have to somehow notify an outside class that we started replaying. Please forgive us for placing
    /// a 'test' propery in here
    /// </summary>
    public delegate void StartedReplayingEventHandler();
}
