using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BackOfficeFrontendService.Commands
{
    public class PrintAdresLabelCommand : DomainCommand
    {
        public long BestellingId { get; set; }

        public PrintAdresLabelCommand() : base(QueueNames.PrintAdresLabel)
        {
        }
    }
}
