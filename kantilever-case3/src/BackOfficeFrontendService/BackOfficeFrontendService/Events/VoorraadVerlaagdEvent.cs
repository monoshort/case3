using System.Diagnostics.CodeAnalysis;
using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.Events
{
    [ExcludeFromCodeCoverage]
    public class VoorraadVerlaagdEvent : DomainEvent
    {
        public VoorraadVerlaagdEvent() : base(TopicNames.VoorraadVerlaagdEvent)
        {
        }

        public long Artikelnummer { get; set; }
        public int Aantal { get; set; }
        public int NieuweVoorraad { get; set; }
    }
}
