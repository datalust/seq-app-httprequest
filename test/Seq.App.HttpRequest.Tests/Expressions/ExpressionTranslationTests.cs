using System.Collections.Generic;
using Seq.App.HttpRequest.Expressions.Compilation;
using Seq.App.HttpRequest.Expressions.Parsing;
using Seq.App.HttpRequest.Tests.Support;
using Xunit;

namespace Seq.App.HttpRequest.Tests.Expressions
{
    public class ExpressionTranslationTests
    {
        public static IEnumerable<object[]> ExpressionEvaluationCases =>
            AsvCases.ReadCases("translation-cases.asv");

        [Theory]
        [MemberData(nameof(ExpressionEvaluationCases))]
        public void ExpressionsAreCorrectlyTranslated(string expr, string expected)
        {
            var parsed = new ExpressionParser().Parse(expr);
            var translated = ExpressionCompiler.Translate(parsed);
            var actual = translated.ToString();
            Assert.Equal(expected, actual);
        }
    }
}
