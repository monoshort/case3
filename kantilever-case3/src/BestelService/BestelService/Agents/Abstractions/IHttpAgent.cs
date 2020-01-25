using System.Threading.Tasks;

namespace BestelService.Agents.Abstractions
{
    /// <summary>
    /// An abstraction of our HTTP library
    /// </summary>
    public interface IHttpAgent
    {
        /// <summary>
        /// Post json data to a specified endpoint and return its response
        /// </summary>
        Task<TReturn> PostAsync<T, TReturn>(string url, T entity);
    }
}
