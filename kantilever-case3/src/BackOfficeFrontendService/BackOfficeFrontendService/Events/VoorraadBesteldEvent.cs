using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.Events
{
    public class VoorraadBesteldEvent : DomainEvent
    {
        public long Artikelnummer { get; set; }
        public long BesteldeVoorraad { get; set; }

        public VoorraadBesteldEvent() : base(TopicNames.VoorraadBesteldEvent)
        {
        }
    }
}
