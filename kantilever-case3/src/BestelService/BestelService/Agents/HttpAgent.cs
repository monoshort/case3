using System.Threading.Tasks;
using BestelService.Agents.Abstractions;
using Flurl.Http;

namespace BestelService.Agents
{
    public class HttpAgent : IHttpAgent
    {
        /// <inheritdoc/>
        public Task<TReturn> PostAsync<T, TReturn>(string url, T entity)
        {
            return url.PostJsonAsync(entity)
                .ReceiveJson<TReturn>();
        }
    }
}
