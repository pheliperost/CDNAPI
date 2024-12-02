using CDNAPI.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestCDNAPI
{
    public class LogServiceTests
    {
        private readonly Mock<ILogRepository> _repositoryMock;
        private readonly Mock<ILogTransformer> _transformerMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly ILogService _logService;

        public LogServiceTests()
        {
            _repositoryMock = new Mock<ILogRepository>();
            _transformerMock = new Mock<ILogTransformer>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _logService = new LogService(_repositoryMock.Object, _transformerMock.Object, _httpClientFactoryMock.Object);
        }

        [Fact]
        public async Task TransformLogAsync_WithValidUrl_ReturnsTransformedContent()
        {
            // Arrange
            var request = new LogTransformRequest
            {
                Input = "http://example.com/log.txt",
                InputType = "url",
                OutputFormat = "response"
            };

            var httpClientMock = new Mock<HttpClient>();
            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClientMock.Object);

            _transformerMock.Setup(x => x.Transform(It.IsAny<string>()))
                .Returns("transformed content");

            // Act
            var result = await _logService.TransformLogAsync(request);

            // Assert
            Assert.Equal("transformed content", result);
        }

        [Fact]
        public async Task GetSavedLogByIdAsync_ExistingId_ReturnsLog()
        {
            // Arrange
            var id = Guid.NewGuid();
            var log = new Log { Id = id, OriginalContent = "test content" };
            _repositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(log);

            // Act
            var result = await _logService.GetSavedLogByIdAsync(id);

            // Assert
            Assert.Equal(log, result);
        }
    }
}
