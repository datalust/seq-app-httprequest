using System.Threading.Tasks;
using Seq.Apps;
using Serilog.Events;

// ReSharper disable UnusedAutoPropertyAccessor.Global, MemberCanBePrivate.Global

namespace Seq.App.Http
{
    [SeqApp("HTTP",
        Description = "Send events and notifications from Seq to a remote HTTP/REST/WebHook endpoint.")]
    public class HttpApp : SeqApp, ISubscribeToAsync<LogEvent>
    {
        readonly HttpAppClient _client;

        HttpRequestMessageFactory? _httpRequestMessageFactory;

        public HttpApp()
            : this(new RuntimeHttpAppClient())
        {
        }

        internal HttpApp(HttpAppClient client)
        {
            _client = client;
        }
        
        [SeqAppSetting(HelpText = "The target URL. May include template substitutions based on event properties, for " +
                                  "example, `https://api.example.com/notify?to={Email}`. Placeholders in templates will" +
                                  " be URI-encoded.")]
        public string? Url { get; set; }

        [SeqAppSetting(IsOptional = true, HelpText = "The HTTP method to use. The default is `POST`.")]
        public HttpMethodSetting Method { get; set; } = HttpMethodSetting.POST;

        [SeqAppSetting(InputType = SettingInputType.LongText, IsOptional = true,
            HelpText = "The request body to send. Template based on event properties, supporting plain text and JSON.")]
        public string? Body { get; set; }

        [SeqAppSetting(IsOptional = true, DisplayName = "Media Type",
            HelpText = "Media type describing the body.")]
        public string? MediaType { get; set; }
        
        [SeqAppSetting(InputType = SettingInputType.Password, IsOptional = true, DisplayName = "Authentication Header",
            HelpText = "An optional `Name: Value` header, stored as sensitive data, for authentication purposes.")]
        public string? AuthenticationHeader { get; set; }

        [SeqAppSetting(InputType = SettingInputType.LongText, IsOptional = true, DisplayName = "Other Headers",
            HelpText = "Additional headers to send with the request, one per line in `Name: Value` format.")]
        public string? OtherHeaders { get; set; }
        
        [SeqAppSetting(InputType = SettingInputType.Checkbox, DisplayName = "Extended Error Diagnostics",
            HelpText = "Whether or not to include outbound request bodies, URLs, etc., and response bodies when requests fail.")]
        public bool ExtendedErrorDiagnostics { get; set; }
        
        protected override void OnAttached()
        {
            _httpRequestMessageFactory = new HttpRequestMessageFactory(
                Url,
                Method,
                Body,
                MediaType,
                AuthenticationHeader, OtherHeaders);
        }

        public async Task OnAsync(Event<LogEvent> evt)
        {
            var message = _httpRequestMessageFactory!.FromEvent(evt.Data);
            var response = await _client.SendAsync(message);
            if (response.IsSuccessStatusCode)
                return;

            var log = Log;
            if (ExtendedErrorDiagnostics)
            {
                log = log
                    .ForContext("RequestUrl", message.RequestUri)
                    .ForContext("ResponseBody", await response.Content.ReadAsStringAsync());
            }
            
            log.Error("Outbound HTTP request failed with status code {StatusCode}", response.StatusCode);
        }
    }
}
