using System.Diagnostics.CodeAnalysis;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Models;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.Events
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
