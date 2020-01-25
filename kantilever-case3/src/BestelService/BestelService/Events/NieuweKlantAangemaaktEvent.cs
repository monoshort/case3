using System.Diagnostics.CodeAnalysis;
using BestelService.Constants;
using BestelService.Core.Models;
using Minor.Miffy.MicroServices.Events;

namespace BestelService.Events
{
    [ExcludeFromCodeCoverage]
    public class NieuweKlantAangemaaktEvent : DomainEvent
    {
        public Klant Klant { get; set; }

        public NieuweKlantAangemaaktEvent() : base(TopicNames.NieuweKlantAangemaakt)
        {
        }
    }
}
