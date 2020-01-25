using BackOfficeFrontendService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace BackOfficeFrontendService.Commands
{
    public class ControleerOfErWanbetalingenZijnCommand : DomainCommand
    {
        public ControleerOfErWanbetalingenZijnCommand() : base(QueueNames.ControleerOfErWanbetalingenZijn)
        {
        }
    }
}
