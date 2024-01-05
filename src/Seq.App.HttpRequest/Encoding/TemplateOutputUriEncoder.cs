using System;
using Seq.Syntax.Templates.Encoding;

namespace Seq.App.HttpRequest.Encoding
{
    class TemplateOutputUriEncoder: TemplateOutputEncoder
    {
        public override string Encode(string value) => Uri.EscapeDataString(value);
    }
}
