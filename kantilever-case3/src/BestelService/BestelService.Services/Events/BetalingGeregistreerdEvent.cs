using System.Diagnostics.CodeAnalysis;
using BestelService.Services.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BestelService.Services.Events
{
    [ExcludeFromCodeCoverage]
    public class BetalingGeregistreerdEvent : DomainEvent
    {
        public long BestellingId { get; set; }
        public decimal OpenstaandBedrag { get; set; }

        public BetalingGeregistreerdEvent() : base(TopicNames.BetalingGeregistreerd)
        {
        }
    }
}
