using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Notes.Helpers;
using Notes.StringConstants;

namespace Notes.Services
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient httpClient;
        private readonly CancellationToken token;
        private readonly Dictionary<string, string> baseHeade;

        public RestClient()
        {
            var handler = new HttpClientHandler();
            httpClient = new HttpClient(handler);
            token = default;
            baseHeade = new Dictionary<string, string>() { { "Authorization", $"Bearer {Constants.ApiKey}" } };
        }

        public async Task<TryResult<TResponse>> GetAsync<TResponse>(string url)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    if (baseHeade != null)
                    {
                        foreach (var customHeader in baseHeade)
                        {
                            request.Headers.Add(customHeader.Key, customHeader.Value);
                        }
                    }

                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        string responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        if (!response.IsSuccessStatusCode)
                        {
                            System.Diagnostics.Debug.WriteLine($"Request: {url}");
                            System.Diagnostics.Debug.WriteLine($"Response: {responseJson}");
                            return TryResult.Unsucceed<TResponse>();
                        }

                        return TryResult.Success(JsonConvert.DeserializeObject<TResponse>(responseJson));
                    }
                }
            }
            catch
            {
                return TryResult.Unsucceed<TResponse>();
            }
        }

        public async Task<TryResult<TResponse>> PostAsync<TRequest, TResponse>(TRequest data, string url)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    List<byte> byteData = new List<byte>();
                    var jsonSerialization = JsonConvert.SerializeObject(data);

                    request.Content = new StringContent(jsonSerialization, Encoding.UTF8, "application/json");

                    if (baseHeade != null)
                    {
                        foreach (var customHeader in baseHeade)
                        {
                            request.Headers.Add(customHeader.Key, customHeader.Value);
                        }
                    }

                    using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false))
                    {
                        var total = response.Content.Headers.ContentLength ?? -1L;
                        var canReportProgress = total != -1;

                        using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            var totalRead = 0L;
                            var buffer = new byte[1024];
                            var isMoreToRead = true;

                            do
                            {
                                token.ThrowIfCancellationRequested();

                                var read = await stream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);
                                if (read == 0)
                                {
                                    isMoreToRead = false;
                                }
                                else
                                {
                                    if (read != buffer.Length)
                                    {
                                        byte[] cpArray = new byte[read];
                                        Array.Copy(buffer, cpArray, read);
                                        byteData.AddRange(cpArray);
                                    }
                                    else
                                    {
                                        byteData.AddRange(buffer);
                                    }

                                    totalRead += read;
                                }
                            }
                            while (isMoreToRead);
                        }

                        var responseJson = Encoding.UTF8.GetString(byteData.ToArray(), 0, byteData.Count);

                        var statusCode = response.EnsureSuccessStatusCode().StatusCode;
                        if (statusCode != HttpStatusCode.OK)
                        {
                            System.Diagnostics.Debug.WriteLine($"Request: {url}, {jsonSerialization}");
                            System.Diagnostics.Debug.WriteLine($"Response: {responseJson}");
                            return TryResult.Unsucceed<TResponse>();
                        }

                        return TryResult.Success(JsonConvert.DeserializeObject<TResponse>(responseJson));
                    }
                }
            }
            catch
            {
                return TryResult.Unsucceed<TResponse>();
            }
        }
    }
}
