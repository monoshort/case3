using FrontendService.Constants;
using FrontendService.Models;
using Minor.Miffy.MicroServices.Commands;

namespace FrontendService.Commands
{
    public class MaakNieuweBestellingAanCommand : DomainCommand
    {
        public Bestelling Bestelling { get; set; }

        public MaakNieuweBestellingAanCommand() : base(QueueNames.MaakNieuweBestellingAanCommand)
        {
        }
    }
}
