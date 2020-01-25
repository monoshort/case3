using BestelService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BestelService.Commands
{
    public class KeurBestellingAfCommand : DomainCommand
    {
        public long BestellingId { get; set; }

        public KeurBestellingAfCommand() : base(QueueNames.KeurBestellingAf)
        {
        }
    }
}
