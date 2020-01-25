using System.Diagnostics.CodeAnalysis;
using FrontendService.Constants;
using FrontendService.Models;
using Minor.Miffy.MicroServices.Events;

namespace FrontendService.Events
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
