using CDNAPI.Interfaces;
using CDNAPI.Utils;
using System;
using Xunit;

namespace XUnitTestCDNAPI
{
    public class LogTransformerTests
    {
        private readonly ILogTransformer _transformer;

        public LogTransformerTests()
        {
            _transformer = new LogTransformer();
        }

        [Fact]
        public void Transform_ValidInput_ReturnsCorrectFormat()
        {
            // Arrange
            var input = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";

            // Act
            var result = _transformer.Transform(input);

            // Assert
            Assert.Contains("\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT", result);
        }

        [Fact]
        public void Transform_InvalidateStatus_ReturnsRefreshHit()
        {
            // Arrange
            var input = "312|200|INVALIDATE|\"GET /robots.txt HTTP/1.1\"|100.2";

            // Act
            var result = _transformer.Transform(input);

            // Assert
            Assert.Contains("REFRESH_HIT", result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Transform_EmptyOrNullInput_ThrowsArgumentException(string input)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _transformer.Transform(input));
        }
    }
}
