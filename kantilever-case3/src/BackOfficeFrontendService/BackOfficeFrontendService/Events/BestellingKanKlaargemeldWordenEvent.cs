using System.Diagnostics.CodeAnalysis;
using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.Events
{
    [ExcludeFromCodeCoverage]
    public class BestellingKanKlaarGemeldWordenEvent : DomainEvent
    {
        public long BestellingId { get; set; }

        public BestellingKanKlaarGemeldWordenEvent() : base(TopicNames.BestellingKanKlaarGemeldWorden)
        {
        }
    }

}
