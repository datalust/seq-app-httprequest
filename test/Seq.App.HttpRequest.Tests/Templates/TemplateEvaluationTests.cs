using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Seq.App.HttpRequest.Templates;
using Seq.App.HttpRequest.Tests.Support;
using Xunit;

namespace Seq.App.HttpRequest.Tests.Templates
{
    public class TemplateEvaluationTests
    {
        public static IEnumerable<object[]> TemplateEvaluationCases =>
            AsvCases.ReadCases("template-evaluation-cases.asv");

        [Theory]
        [MemberData(nameof(TemplateEvaluationCases))]
        public void TemplatesAreCorrectlyEvaluated(string template, string expected)
        {
            var evt = Some.InformationEvent("Hello, {Name}!", "nblumhardt");
            var frFr = CultureInfo.GetCultureInfoByIetfLanguageTag("fr-FR");
            var compiled = new ExpressionTemplate(template, culture: frFr);
            var output = new StringWriter();
            compiled.Format(evt, output);
            var actual = output.ToString();
            Assert.Equal(expected, actual);
        }
    }
}
