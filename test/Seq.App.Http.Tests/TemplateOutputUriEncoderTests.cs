using Seq.App.Http.Encoding;
using Xunit;

namespace Seq.App.Http.Tests
{
    public class TemplateOutputUriEncoderTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("test", "test")]
        [InlineData("test data", "test%20data")]
        [InlineData("&c.", "%26c.")]
        [InlineData("a/b/c", "a%2Fb%2Fc")]
        public void OutputIsUriEncoded(string raw, string encoded)
        {
            var encoder = new TemplateOutputUriEncoder();
            var actual = encoder.Encode(raw);
            Assert.Equal(encoded, actual);
        }
    }
}