using System.Threading.Tasks;
using Seq.Apps;
using Seq.Apps.LogEvents;

// ReSharper disable UnusedAutoPropertyAccessor.Global, MemberCanBePrivate.Global

namespace Seq.App.Http
{
    [SeqApp("HTTP",
        Description = "Send events and notifications from Seq to a remote HTTP/REST/WebHook endpoint.")]
    public class HttpApp : SeqApp, ISubscribeToAsync<LogEventData>
    {
        readonly HttpAppClient _client;

        public HttpApp()
        {
            
        }

        internal HttpApp(HttpAppClient client)
        {
            _client = client;
        }
        
        protected override void OnAttached()
        {
            base.OnAttached();
        }

        public async Task OnAsync(Event<LogEventData> evt)
        {
            throw new System.NotImplementedException();
        }
    }
}
