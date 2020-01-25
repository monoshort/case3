using System.Diagnostics.CodeAnalysis;
using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.Events
{
    [ExcludeFromCodeCoverage]
    public class BestellingKlaarGemeldEvent : DomainEvent
    {
        public long BestellingId { get; set; }

        public BestellingKlaarGemeldEvent() : base(TopicNames.BestellingKlaarGemeld)
        {
        }
    }
}
