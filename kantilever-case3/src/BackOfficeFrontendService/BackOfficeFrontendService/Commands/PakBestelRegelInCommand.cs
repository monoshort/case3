using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BackOfficeFrontendService.Commands
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
