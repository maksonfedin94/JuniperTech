using System.Threading.Tasks;
using Notes.Helpers;

namespace Notes.Services
{
    public interface IRestClient
    {
        Task<TryResult<TResponse>> GetAsync<TResponse>(string url);

        Task<TryResult<TResponse>> PostAsync<TRequest, TResponse>(TRequest data, string url);
    }
}
