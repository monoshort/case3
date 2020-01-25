using BestelService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BestelService.Commands
{
    public class RegistreerBetalingCommand : DomainCommand
    {
        public decimal BetaaldBedrag { get; set; }
        public string BestellingNummer { get; set; }

        public RegistreerBetalingCommand() : base(QueueNames.RegistreerBetaling)
        {
        }
    }
}
