using System.IO;
using Seq.App.Http.Expressions;
using Seq.App.Http.Templates.Compilation;

namespace Seq.App.Http.Templates.Encoding
{
    class EncodedCompiledTemplate : CompiledTemplate
    {
        readonly CompiledTemplate _inner;
        readonly TemplateOutputEncoder _encoder;

        public EncodedCompiledTemplate(CompiledTemplate inner, TemplateOutputEncoder encoder)
        {
            _inner = inner;
            _encoder = encoder;
        }

        public override void Evaluate(EvaluationContext ctx, TextWriter output)
        {
            var buffer = new StringWriter(output.FormatProvider);
            _inner.Evaluate(ctx, buffer);
            var encoded = _encoder.Encode(buffer.ToString());
            output.Write(encoded);
        }
    }
}