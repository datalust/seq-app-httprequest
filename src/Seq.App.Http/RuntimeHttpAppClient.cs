using System.Net.Http;
using System.Threading.Tasks;

namespace Seq.App.Http
{
    // ReSharper disable once InconsistentNaming
    class RuntimeHttpAppClient : HttpAppClient
    {
        readonly HttpClient _httpClient = new();
        
        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage message)
        {
            return _httpClient.SendAsync(message);
        }

        public override void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}