using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Seq.App.Http.Tests.Support
{
    class TestHttpAppClient : HttpAppClient
    {
        public List<HttpRequestMessage> Received { get; } = new();
        
        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message)
        {
            Received.Add(message);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
