using System.Diagnostics.CodeAnalysis;
using BestelService.Services.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BestelService.Services.Events
{
    [ExcludeFromCodeCoverage]
    public class BestellingFactuurGeprintEvent : DomainEvent
    {
        public long BestellingId { get; set; }

        public BestellingFactuurGeprintEvent() : base(TopicNames.FactuurGeprint)
        {
        }
    }
}
