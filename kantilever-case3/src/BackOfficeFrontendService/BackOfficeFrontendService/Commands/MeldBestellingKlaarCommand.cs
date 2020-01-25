using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BackOfficeFrontendService.Commands
{
    public class MeldBestellingKlaarCommand : DomainCommand
    {
        public long BestellingId { get; set; }

        public MeldBestellingKlaarCommand() : base(QueueNames.MeldBestellingKlaar)
        {
        }
    }
}
