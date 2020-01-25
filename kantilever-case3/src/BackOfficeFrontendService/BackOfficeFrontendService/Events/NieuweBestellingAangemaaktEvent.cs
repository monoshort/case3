using System.Diagnostics.CodeAnalysis;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Models;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.Events
{
    [ExcludeFromCodeCoverage]
    public class NieuweBestellingAangemaaktEvent : DomainEvent
    {
        public NieuweBestellingAangemaaktEvent() : base(TopicNames.NieuweBestellingAangemaakt)
        {
        }

        public Bestelling Bestelling { get; set; }
    }
}
