using System.Diagnostics.CodeAnalysis;
using FrontendService.Constants;
using Minor.Miffy.MicroServices.Events;

namespace FrontendService.Events
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
