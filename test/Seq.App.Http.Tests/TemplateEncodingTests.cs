using System.Collections.Generic;
using System.IO;
using Seq.App.Http.Templates;
using Seq.App.Http.Tests.Support;
using Xunit;

namespace Seq.App.Http.Tests
{
    public class TemplateEncodingTests
    {
        public static IEnumerable<object[]> TemplateEvaluationCases =>
            AsvCases.ReadCases("template-encoding-cases.asv");

        [Theory]
        [MemberData(nameof(TemplateEvaluationCases))]
        public void TemplatesAreCorrectlyEvaluated(string template, string expected)
        {
            var evt = Some.InformationEvent("Hello, {Name}!", "nblumhardt");
            var compiled = new ExpressionTemplate(template, encoder: new ParenthesizingEncoder());
            var output = new StringWriter();
            compiled.Format(evt, output);
            var actual = output.ToString();
            Assert.Equal(expected, actual);
        }
    }
}
