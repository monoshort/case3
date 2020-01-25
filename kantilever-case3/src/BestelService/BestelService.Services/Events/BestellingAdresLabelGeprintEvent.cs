using System.Diagnostics.CodeAnalysis;
using BestelService.Services.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BestelService.Services.Events
{
    [ExcludeFromCodeCoverage]
    public class BestellingAdresLabelGeprintEvent : DomainEvent
    {
        public long BestellingId { get; set; }

        public BestellingAdresLabelGeprintEvent() : base(TopicNames.AdresLabelGeprint)
        {
        }
    }
}
