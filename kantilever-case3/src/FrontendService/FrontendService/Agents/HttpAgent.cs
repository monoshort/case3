using System.Threading.Tasks;
using Flurl.Http;
using FrontendService.Agents.Abstractions;

namespace FrontendService.Agents
{
    public class HttpAgent : IHttpAgent
    {
        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(string url)
        {
            return await url.GetJsonAsync<T>();
        }

        /// <inheritdoc/>
        public Task<TReturn> PostAsync<T, TReturn>(string url, T entity)
        {
            return url.PostJsonAsync(entity)
                .ReceiveJson<TReturn>();
        }
    }
}
