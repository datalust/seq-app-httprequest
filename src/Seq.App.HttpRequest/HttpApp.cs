using System;
using System.Net.Http;
using System.Threading.Tasks;
using Seq.App.HttpRequest.Settings;
using Seq.Apps;
using Serilog.Events;

// ReSharper disable UnusedAutoPropertyAccessor.Global, MemberCanBePrivate.Global

namespace Seq.App.HttpRequest
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
        
        [SeqAppSetting(DisplayName = "URL",
            Syntax = "template",
            HelpText = "The target URL. May include template substitutions based on event properties, for " +
                       "example, `https://api.example.com/notify?to={Email}`. Placeholders in templates will" +
                       " be URI-encoded.")]
        public string? Url { get; set; }

        [SeqAppSetting(
            IsOptional = true, 
            Syntax = "code",
            HelpText = "The HTTP method to use. The default is `POST`.")]
        public HttpMethodSetting Method { get; set; } = HttpMethodSetting.POST;

        [SeqAppSetting(InputType = SettingInputType.LongText, IsOptional = true,
            Syntax = "template",
            HelpText = "The request body to send.")]
        public string? Body { get; set; }
        
        [SeqAppSetting(InputType = SettingInputType.Checkbox, IsOptional = true, DisplayName = "Body is a template",
            HelpText = "Treat the request body (above) as a template over event properties.")]
        public bool BodyIsTemplate { get; set; }

        [SeqAppSetting(IsOptional = true, DisplayName = "Media Type",
            Syntax = "code",
            HelpText = "Media type describing the body.")]
        public string? MediaType { get; set; }
        
        [SeqAppSetting(InputType = SettingInputType.Password, IsOptional = true, DisplayName = "Authentication Header",
            Syntax = "code",
            HelpText = "An optional `Name: Value` header, stored as sensitive data, for authentication purposes.")]
        public string? AuthenticationHeader { get; set; }

        [SeqAppSetting(InputType = SettingInputType.LongText, IsOptional = true, DisplayName = "Other Headers",
            Syntax = "code",
            HelpText = "Additional headers to send with the request, one per line in `Name: Value` format.")]
        public string? OtherHeaders { get; set; }
        
        [SeqAppSetting(InputType = SettingInputType.Checkbox, IsOptional = true, DisplayName = "Extended Error Diagnostics",
            HelpText = "Whether or not to include outbound request bodies, URLs, etc., and response bodies when requests fail.")]
        public bool ExtendedErrorDiagnostics { get; set; }
        
        protected override void OnAttached()
        {
            _httpRequestMessageFactory = new HttpRequestMessageFactory(
                Url ?? throw new InvalidOperationException("The `Url` setting is required."),
                new HttpMethod(Method.ToString()),
                Body,
                BodyIsTemplate,
                MediaType,
                HeaderSettingFormat.FromSettings(AuthenticationHeader, OtherHeaders));
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
