using Seq.App.HttpRequest.Templates;
using Xunit;

namespace Seq.App.HttpRequest.Tests.Templates
{
    public class ExpressionTemplateTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("{", "{{")]
        [InlineData("}", "}}")]
        [InlineData("a{b}c{{d}}", "a{{b}}c{{{{d}}}}")]
        public void LiteralTextCanBeEscaped(string literal, string escaped)
        {
            var actual = ExpressionTemplate.EscapeLiteralText(literal);
            Assert.Equal(escaped, actual);
        }
    }
}