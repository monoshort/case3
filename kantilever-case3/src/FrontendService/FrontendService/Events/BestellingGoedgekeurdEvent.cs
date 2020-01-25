using System.Diagnostics.CodeAnalysis;
using FrontendService.Constants;
using Minor.Miffy.MicroServices.Events;

namespace FrontendService.Events
{
    [ExcludeFromCodeCoverage]
    public class BestellingGoedgekeurdEvent : DomainEvent
    {
        public long BestellingId { get; set; }

        public BestellingGoedgekeurdEvent() : base(TopicNames.BestellingGoedgekeurd)
        {
        }
    }
}
