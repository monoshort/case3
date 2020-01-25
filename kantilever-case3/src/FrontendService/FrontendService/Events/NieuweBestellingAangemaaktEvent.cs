using System.Diagnostics.CodeAnalysis;
using FrontendService.Constants;
using FrontendService.Models;
using Minor.Miffy.MicroServices.Events;

namespace FrontendService.Events
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
