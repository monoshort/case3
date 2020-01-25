using System.Diagnostics.CodeAnalysis;
using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.Events
{
    [ExcludeFromCodeCoverage]
    public class BestellingAdresLabelGeprintEvent : DomainEvent
    {
        public long BestellingId { get; set; }

        public BestellingAdresLabelGeprintEvent() : base(TopicNames.BestellingAdresLabelGeprint)
        {
        }
    }
}
