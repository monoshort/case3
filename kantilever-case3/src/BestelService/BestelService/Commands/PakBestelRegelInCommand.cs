using BestelService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BestelService.Commands
{
    public class PakBestelRegelInCommand : DomainCommand
    {
        public long BestelRegelId { get; set; }
        public long BestellingId { get; set; }

        public PakBestelRegelInCommand() : base(QueueNames.PakBestelRegelIn)
        {
        }
    }
}
