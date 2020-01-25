using System.Threading.Tasks;

namespace BackOfficeFrontendService.Agents.Abstractions
{
    public interface IHttpAgent
    {
        /// <summary>
        /// Send a HTTP GET request asynchronously
        /// </summary>
        Task<T> GetAsync<T>(string url);

        /// <summary>
        /// Send a HTTP POST request asynchronously
        /// </summary>
        Task<TReturn> PostAsync<T, TReturn>(string url, T entity);
    }
}
