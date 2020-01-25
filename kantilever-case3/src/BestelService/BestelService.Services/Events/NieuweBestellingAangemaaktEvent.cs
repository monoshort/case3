using System.Diagnostics.CodeAnalysis;
using BestelService.Core.Models;
using BestelService.Services.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BestelService.Services.Events
{
    [ExcludeFromCodeCoverage]
    public class NieuweBestellingAangemaaktEvent : DomainEvent
    {
        public Bestelling Bestelling { get; set; }

        public NieuweBestellingAangemaaktEvent() : base(TopicNames.NieuweBestellingAangemaakt)
        {
        }
    }
}
