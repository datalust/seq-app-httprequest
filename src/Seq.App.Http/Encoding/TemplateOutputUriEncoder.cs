using System;
using Seq.App.Http.Templates.Encoding;

namespace Seq.App.Http.Encoding
{
    class TemplateOutputUriEncoder: TemplateOutputEncoder
    {
        public override string Encode(string value) => Uri.EscapeDataString(value);
    }
}
