using BestelService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BestelService.Commands
{
    public class PrintAdresLabelCommand : DomainCommand
    {
        public long BestellingId { get; set; }

        public PrintAdresLabelCommand() : base(QueueNames.PrintAdresLabel)
        {
        }
    }
}
