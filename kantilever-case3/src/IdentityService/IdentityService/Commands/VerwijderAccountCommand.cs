using IdentityService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace IdentityService.Commands
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