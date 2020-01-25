using BestelService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BestelService.Commands
{
    public class ControleerOfErWanbetalingenZijnCommand : DomainCommand
    {
        public ControleerOfErWanbetalingenZijnCommand() : base(QueueNames.ControleerOfErWanbetalingenZijn)
        {
        }
    }
}
