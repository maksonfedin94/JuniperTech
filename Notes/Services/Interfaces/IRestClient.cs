using System.Threading;
using System.Threading.Tasks;
using Notes.Helpers;

namespace Notes.Services
{
    public interface IRestClient
    {
        Task<TryResult<TResponse>> GetAsync<TResponse>(string url, CancellationToken token = default);

        Task<TryResult<TResponse>> PostAsync<TRequest, TResponse>(TRequest data, string url, CancellationToken token = default);
    }
}
