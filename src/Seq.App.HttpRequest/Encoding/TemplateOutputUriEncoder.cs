using System;
using Seq.App.HttpRequest.Templates.Encoding;

namespace Seq.App.HttpRequest.Encoding
{
    class TemplateOutputUriEncoder: TemplateOutputEncoder
    {
        public override string Encode(string value) => Uri.EscapeDataString(value);
    }
}
