using System.Diagnostics.CodeAnalysis;
using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.Events
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
