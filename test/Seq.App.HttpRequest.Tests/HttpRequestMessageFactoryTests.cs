using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Seq.App.HttpRequest.Tests.Support;
using Serilog.Events;
using Xunit;

namespace Seq.App.HttpRequest.Tests
{
    public class HttpRequestMessageFactoryTests
    {
        readonly LogEvent _event = Some.InformationEvent("Hello, {Name}!", "world");
        
        [Fact]
        public void NullBodyProducesNoMessageContent()
        {
            var message = CreateMessageFromEvent(body: null);
            Assert.Null(message.Content);
        }

        [Fact]
        public void BodyIsStringContent()
        {
            var message = CreateMessageFromEvent(body: "The name is {Name}", mediaType: "text/plain", method: HttpMethod.Post);
            
            Assert.Equal(HttpMethod.Post, message.Method);

            var content = Assert.IsType<StringContent>(message.Content);
            Assert.Equal("text/plain", content.Headers.ContentType?.MediaType);
            Assert.Equal("utf-8", content.Headers.ContentType?.CharSet);
            
            Assert.Equal("The name is world", new StreamReader(content.ReadAsStream()).ReadToEnd());
        }

        [Fact]
        public void RequestUrlIsTemplated()
        {
            var message = CreateMessageFromEvent(url: "https://{ToLower(Name)}.example.com/{1 + 1}");
            Assert.Equal("https://world.example.com/2", message.RequestUri?.ToString());
        }
        
        [Fact]
        public void RequestUrlEncodesSubstitutions()
        {
            var message = CreateMessageFromEvent(url: "https://example.com/{'a b'}");
            // ToString() flattens unnecessary encodings like the one used here :-)
            Assert.Equal("https://example.com/a%20b", message.RequestUri?.OriginalString);
        }

        [Fact]
        public void HeadersAreAttached()
        {
            var message = CreateMessageFromEvent(headers: new List<(string, string)>
            {
                ("X-Test-A", "a"),
                ("Authorization", "Basic YWxhZGRpbjpvcGVuc2VzYW1l")
            });

            var headers = message.Headers.ToDictionary(h => h.Key, h => h.Value);
            
            Assert.True(headers.TryGetValue("X-Test-A", out var a));
            Assert.Equal("a", a!.FirstOrDefault());
            
            Assert.True(headers.TryGetValue("Authorization", out var az));
            Assert.Equal("Basic YWxhZGRpbjpvcGVuc2VzYW1l", az!.FirstOrDefault());
        }

        HttpRequestMessage CreateMessageFromEvent(string? url = null, HttpMethod? method = null,
            string? body = null, string? mediaType = null, List<(string, string)>? headers = null)
        {
            var factory = new HttpRequestMessageFactory(
                url ?? "https://example.com",
                method ?? HttpMethod.Get,
                body,
                mediaType,
                headers ?? new List<(string, string)>());

            return factory.FromEvent(_event);
        }
    }
}