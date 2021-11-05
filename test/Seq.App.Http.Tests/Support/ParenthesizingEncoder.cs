using Seq.App.Http.Templates.Encoding;

namespace Seq.App.Http.Tests.Support
{
    public class ParenthesizingEncoder : TemplateOutputEncoder
    {
        public override string Encode(string value)
        {
            return $"({value})";
        }
    }
}