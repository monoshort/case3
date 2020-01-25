using KlantService.Constants;
using KlantService.Models;
using Minor.Miffy.MicroServices.Events;

namespace KlantService.Events
{
    public class NieuweKlantAangemaaktEvent : DomainEvent
    {
        public Klant Klant { get; set; }

        public NieuweKlantAangemaaktEvent() : base(TopicNames.NieuweKlantAangemaakt)
        {
        }
    }
}
