using System.Diagnostics.CodeAnalysis;
using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.Events
{
    [ExcludeFromCodeCoverage]
    public class BestellingAfgekeurdEvent : DomainEvent
    {
        public BestellingAfgekeurdEvent() : base(TopicNames.BestellingAfgekeurd)
        {
        }

        public long BestellingId { get; set; }
    }
}
