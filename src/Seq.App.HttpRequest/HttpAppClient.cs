using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Seq.App.HttpRequest
{
    abstract class HttpAppClient: IDisposable
    {
        public abstract Task<HttpResponseMessage> SendAsync(HttpRequestMessage message);

        public virtual void Dispose()
        {
        }
    }
}