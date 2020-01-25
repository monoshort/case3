using System.Diagnostics.CodeAnalysis;
using BestelService.Services.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BestelService.Services.Events
{
    [ExcludeFromCodeCoverage]
    public class BestelRegelIngepaktEvent : DomainEvent
    {
        public long BestelRegelId { get; set; }
        public long BestellingId { get; set; }

        public BestelRegelIngepaktEvent() : base(TopicNames.BestelRegelIngepakt)
        {
        }
    }
}
