using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BackOfficeFrontendService.Commands
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
