using BestelService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BestelService.Commands
{
    public class KeurBestellingGoedCommand : DomainCommand
    {
        public long BestellingId { get; set; }

        public KeurBestellingGoedCommand() : base(QueueNames.KeurBestellingGoed)
        {
        }
    }
}