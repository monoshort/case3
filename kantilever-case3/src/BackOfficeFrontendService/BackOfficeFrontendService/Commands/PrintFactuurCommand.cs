using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BackOfficeFrontendService.Commands
{
    public class PrintFactuurCommand : DomainCommand
    {
        public long BestellingId { get; set; }

        public PrintFactuurCommand() : base(QueueNames.PrintFactuur)
        {
        }
    }
}
