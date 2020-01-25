using System.Diagnostics.CodeAnalysis;
using FrontendService.Constants;
using FrontendService.Models;
using Minor.Miffy.MicroServices.Commands;

namespace FrontendService.Commands
{
    [ExcludeFromCodeCoverage]
    public class MaakNieuweKlantAanCommand : DomainCommand
    {
        public Klant Klant { get; set; }

        public MaakNieuweKlantAanCommand() : base(QueueNames.MaakNieuweKlantAanCommand)
        {
        }
    }
}
