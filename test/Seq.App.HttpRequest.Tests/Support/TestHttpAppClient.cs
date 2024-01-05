using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Seq.App.HttpRequest.Tests.Support
{
    class TestHttpAppClient : HttpAppClient
    {
        public List<HttpRequestMessage> Received { get; } = new();
        
        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage message)
        {
            Received.Add(message);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }
}
