using BestelService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BestelService.Commands
{
    public class MeldBestellingKlaarCommand : DomainCommand
    {
        public long BestellingId { get; set; }

        public MeldBestellingKlaarCommand() : base(QueueNames.MeldBestellingKlaar)
        {
        }
    }
}
