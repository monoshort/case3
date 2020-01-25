using System.Diagnostics.CodeAnalysis;
using FrontendService.Constants;
using Minor.Miffy.MicroServices.Events;

namespace FrontendService.Events
{
    [ExcludeFromCodeCoverage]
    public class BestellingAfgekeurdEvent : DomainEvent
    {
        public long BestellingId { get; set; }

        public BestellingAfgekeurdEvent() : base(TopicNames.BestellingAfgekeurd)
        {
        }
    }
}
