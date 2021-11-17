using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Seq.App.HttpRequest.Encoding;
using Seq.App.HttpRequest.Expressions;
using Seq.App.HttpRequest.Templates;
using Serilog.Events;
using Serilog.Formatting;

namespace Seq.App.HttpRequest
{
    class HttpRequestMessageFactory
    {
        readonly string? _mediaType;
        readonly ExpressionTemplate _url;
        readonly ExpressionTemplate? _body;
        readonly HttpMethod _method;
        readonly List<(string, string)> _headers;
        readonly System.Text.Encoding _utf8 = new UTF8Encoding(false);

        public HttpRequestMessageFactory(string urlTemplate, HttpMethod method, string? body, bool bodyIsTemplate, string? mediaType, List<(string, string)> headers)
        {
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            
            _mediaType = mediaType;
            _headers = headers;
            
            _url = new ExpressionTemplate(urlTemplate, encoder: new TemplateOutputUriEncoder());

            if (body != null || mediaType != null)
            {
                var bodyTemplate = body ?? "";
                if (!bodyIsTemplate)
                    bodyTemplate = ExpressionTemplate.EscapeLiteralText(bodyTemplate);

                _body = new ExpressionTemplate(bodyTemplate);
            }
            else
            {
                _body = null;
            }
            
            _method = method;
        }

        public HttpRequestMessage FromEvent(LogEvent evt)
        {
            var url = Format(_url, evt);

            var message = new HttpRequestMessage(_method, url);

            if (_body != null)
            {
                var body = Format(_body, evt);
                message.Content = new StringContent(body, _utf8, _mediaType);
            }
            
            foreach (var (name, value) in _headers)
            {
                message.Headers.Add(name, value);
            }

            return message;
        }

        static string Format(ITextFormatter template, LogEvent evt)
        {
            var writer = new StringWriter();
            template.Format(evt, writer);
            return writer.ToString();
        }
    }
}
