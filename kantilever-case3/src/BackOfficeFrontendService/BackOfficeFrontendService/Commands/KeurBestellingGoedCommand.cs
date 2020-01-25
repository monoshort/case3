using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BackOfficeFrontendService.Commands
{
    public class KeurBestellingGoedCommand : DomainCommand
    {
        public KeurBestellingGoedCommand() : base(QueueNames.KeurBestellingGoed)
        {
        }

        public long BestellingId { get; set; }
    }
}