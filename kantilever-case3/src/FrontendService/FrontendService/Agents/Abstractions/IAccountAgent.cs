using System.Threading.Tasks;

namespace FrontendService.Agents.Abstractions
{
    public interface IAccountAgent
    {
        /// <summary>
        /// Create an account
        /// </summary>
        Task MaakAccountAanAsync(string username, string password);

        /// <summary>
        /// Delete an account
        /// </summary>
        Task VerwijderAccountAsync(string username);
    }
}
