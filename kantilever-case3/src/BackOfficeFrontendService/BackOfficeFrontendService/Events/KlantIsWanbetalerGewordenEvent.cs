using System.Diagnostics.CodeAnalysis;
using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.Events
{
    [ExcludeFromCodeCoverage]
    public class KlantIsWanbetalerGewordenEvent : DomainEvent
    {
        public long BestellingId { get; set; }

        public KlantIsWanbetalerGewordenEvent() : base(TopicNames.KlantIsWanbetalerGeworden)
        {
        }
    }
}
