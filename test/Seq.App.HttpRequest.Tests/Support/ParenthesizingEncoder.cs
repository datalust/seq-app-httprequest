using Seq.App.HttpRequest.Templates.Encoding;

namespace Seq.App.HttpRequest.Tests.Support
{
    public class ParenthesizingEncoder : TemplateOutputEncoder
    {
        public override string Encode(string value)
        {
            return $"({value})";
        }
    }
}