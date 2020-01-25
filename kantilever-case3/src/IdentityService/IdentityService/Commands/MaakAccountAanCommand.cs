using IdentityService.Constants;
using Minor.Miffy.MicroServices.Commands;

namespace IdentityService.Commands
{
    public class MaakAccountAanCommand : DomainCommand
    {
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Notice: This password is not encrypted due to explicit customer wishes
        ///
        /// Also, encrypting it wouldn't really have a purpose if HTTPS isn't a customr wish either
        /// </summary>
        public string Password { get; set; }

        public MaakAccountAanCommand() : base(QueueNames.MaakAccountAan)
        {
        }
    }
}
