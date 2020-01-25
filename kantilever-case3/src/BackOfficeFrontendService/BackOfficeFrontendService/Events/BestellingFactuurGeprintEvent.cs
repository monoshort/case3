using System.Diagnostics.CodeAnalysis;
using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.Events
{
    [ExcludeFromCodeCoverage]
    public class BestellingFactuurGeprintEvent : DomainEvent
    {
        public long BestellingId { get; set; }

        public BestellingFactuurGeprintEvent() : base(TopicNames.BestellingFactuurGeprint)
        {
        }
    }
}
