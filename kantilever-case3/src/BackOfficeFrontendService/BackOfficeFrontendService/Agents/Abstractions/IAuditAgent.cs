using System.Threading.Tasks;
using BackOfficeFrontendService.Commands;

namespace BackOfficeFrontendService.Agents.Abstractions
{
    public interface IAuditAgent
    {
        /// <summary>
        /// Send an event to the auditlogger to start replaying
        /// </summary>
        /// <returns>The amount of events that will be replayed</returns>
        Task<string> ReplayEventsAsync(ReplayEventsCommand command);
    }
}
