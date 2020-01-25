using BestelService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BestelService.Commands
{
    public class PrintFactuurCommand : DomainCommand
    {
        public long BestellingId { get; set; }

        public PrintFactuurCommand() : base(QueueNames.PrintFactuur)
        {
        }
    }
}
