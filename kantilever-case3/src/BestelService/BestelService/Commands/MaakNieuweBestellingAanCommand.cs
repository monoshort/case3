using BestelService.Constants;
using BestelService.Core.Models;
using Minor.Miffy.MicroServices.Commands;

namespace BestelService.Commands
{
    public class MaakNieuweBestellingAanCommand : DomainCommand
    {
        public Bestelling Bestelling { get; set; }

        public MaakNieuweBestellingAanCommand() : base(QueueNames.MaakNieuweBestellingAan)
        {
        }
    }
}
