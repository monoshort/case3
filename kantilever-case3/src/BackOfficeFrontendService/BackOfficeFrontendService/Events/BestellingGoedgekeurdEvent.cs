using System.Diagnostics.CodeAnalysis;
using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.Events
{
    [ExcludeFromCodeCoverage]
    public class BestellingGoedgekeurdEvent : DomainEvent
    {
        public BestellingGoedgekeurdEvent() : base(TopicNames.BestellingGoedgekeurd)
        {
        }

        public long BestellingId { get; set; }
    }
}
