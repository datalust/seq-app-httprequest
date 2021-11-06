using System;
using System.Net.Http;
using System.Threading.Tasks;
using Seq.App.Http.Settings;
using Seq.App.Http.Tests.Support;
using Seq.Apps;
using Serilog.Events;
using Xunit;

namespace Seq.App.Http.Tests
{
    public class HttpAppTests
    {
        [Fact]
        public async Task EventsAreSentAsMessages()
        {
            var client = new TestHttpAppClient();
            
            var app = new HttpApp(client)
            {
                Url = "https://example.com",
                Method = HttpMethodSetting.GET
            };

            app.Attach(new TestAppHost());

            var evt = Some.InformationEvent();

            await app.OnAsync(new Event<LogEvent>("event-1", 123, DateTime.UtcNow, evt));

            var message = Assert.Single(client.Received);
            Assert.Equal(HttpMethod.Get, message.Method);
            Assert.Equal("https://example.com/", message.RequestUri?.ToString());
        }
    }
}
