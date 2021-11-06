using System.Collections.Generic;
using Seq.App.Http.Expressions.Compilation;
using Seq.App.Http.Expressions.Parsing;
using Seq.App.Http.Tests.Support;
using Xunit;

namespace Seq.App.Http.Tests.Expressions
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
