using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Seq.App.Http.Encoding;
using Seq.App.Http.Templates;
using Serilog.Events;

namespace Seq.App.Http
{
    class HttpRequestMessageFactory
    {
        readonly string? _mediaType;
        readonly ExpressionTemplate _url, _body;
        readonly HttpMethod _method;
        readonly List<(string, string)> _headers;

        public HttpRequestMessageFactory(string? url, HttpMethodSetting method, string? body, string? mediaType, string? authenticationHeader, string? otherHeaders)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            _mediaType = mediaType;
            _url = new ExpressionTemplate(url, encoder: new TemplateOutputUriEncoder());
            _body = new ExpressionTemplate(body ?? "");
            _method = new HttpMethod(method.ToString());
            _headers = new List<(string, string)>();
            if (!string.IsNullOrWhiteSpace(authenticationHeader))
                _headers.Add(HeaderSettingFormat.Parse(authenticationHeader));
            if (!string.IsNullOrWhiteSpace(otherHeaders))
            {
                var reader = new StringReader(otherHeaders);
                var line = reader.ReadLine();
                while (!string.IsNullOrWhiteSpace(line))
                {
                    _headers.Add(HeaderSettingFormat.Parse(line));
                    line = reader.ReadLine();
                }
            }
        }

        public HttpRequestMessage FromEvent(LogEvent evt)
        {
        }
    }

    static class HeaderSettingFormat
    {
        public static (string, string) Parse(string header)
        {
            var colon = header.IndexOf(":", StringComparison.Ordinal);
            if (colon is 0 or -1)
                throw new ArgumentException("The header must be specified in `Name: Value` format.");
            return (header[..colon].Trim(), header[(colon + 1)..].Trim());
        }
    }
}
