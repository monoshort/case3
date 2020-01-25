using System.Threading.Tasks;
using BackOfficeFrontendService.Agents.Abstractions;
using Flurl.Http;

namespace BackOfficeFrontendService.Agents
{
    public class HttpAgent : IHttpAgent
    {
        /// <inheritdoc/>
        public Task<T> GetAsync<T>(string url)
        {
            return url.GetJsonAsync<T>();
        }

        /// <inheritdoc/>
        public Task<TReturn> PostAsync<T, TReturn>(string url, T entity)
        {
            return url.PostJsonAsync(entity)
                .ReceiveJson<TReturn>();
        }
    }
}
