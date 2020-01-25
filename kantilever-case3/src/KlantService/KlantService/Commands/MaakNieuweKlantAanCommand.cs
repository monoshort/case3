using System.Diagnostics.CodeAnalysis;
using KlantService.Constants;
using KlantService.Models;
using Minor.Miffy.MicroServices.Commands;

namespace KlantService.Commands
{
    [ExcludeFromCodeCoverage]
    public class MaakNieuweKlantAanCommand : DomainCommand
    {
        public Klant Klant { get; set; }

        public MaakNieuweKlantAanCommand() : base(QueueNames.MaakNieuweKlantAan)
        {
        }
    }
}
