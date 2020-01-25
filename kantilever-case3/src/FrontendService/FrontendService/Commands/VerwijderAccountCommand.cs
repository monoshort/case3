using FrontendService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace FrontendService.Commands
{
    public class VerwijderAccountCommand : DomainCommand
    {
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }
        
        public VerwijderAccountCommand() : base(QueueNames.VerwijderAccount)
        {
        }
    }
}