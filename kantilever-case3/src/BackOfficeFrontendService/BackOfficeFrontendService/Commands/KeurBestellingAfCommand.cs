using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BackOfficeFrontendService.Commands
{
    public class KeurBestellingAfCommand : DomainCommand
    {
        public long BestellingId { get; set; }

        public KeurBestellingAfCommand() : base(QueueNames.KeurBestellingAf)
        {
        }
    }
}
