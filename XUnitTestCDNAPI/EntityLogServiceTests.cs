using AutoFixture;
using CDNAPI.Interfaces;
using CDNAPI.Models;
using CDNAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using XUnitTestCDNAPI.Utils;

namespace XUnitTestCDNAPI
{
    public class EntityLogServiceTests
    {
        private readonly Mock<IEntityLogRepository> _repositoryMock;
        private readonly Mock<ILogTransformer> _transformerMock;
        private readonly EntityLogService _service;
        private readonly Fixture _fixture;

        public EntityLogServiceTests()
        {
            _repositoryMock = new Mock<IEntityLogRepository>();
            _transformerMock = new Mock<ILogTransformer>();
            _service = new EntityLogService(_repositoryMock.Object, _transformerMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetSavedLogsAsync_ShouldReturnAllLogs()
        {
            // Arrange
            var expectedLogs = _fixture.CreateMany<EntityLog>(3).ToList();
            _repositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _service.GetSavedLogsAsync();

            // Assert
            Assert.Equal(expectedLogs, result);
            _repositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetSavedLogByIdAsync_ShouldReturnLog()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedLog = _fixture.Create<EntityLog>();
            _repositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(expectedLog);

            // Act
            var result = await _service.GetSavedLogByIdAsync(id);

            // Assert
            Assert.Equal(expectedLog, result);
            _repositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetTransformedLogsAsync_ShouldReturnOnlyTransformedLogs()
        {
            // Arrange
            var logs = new List<EntityLog>
        {
            new EntityLog { AgoraLog = "transformed1" },
            new EntityLog { AgoraLog = null },
            new EntityLog { AgoraLog = "transformed2" }
        };
            _repositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(logs);

            // Act
            var result = await _service.GetTransformedLogsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains("transformed1", result);
            Assert.Contains("transformed2", result);
        }

        [Fact]
        public async Task TransformLogFromRequest_WithFileOutput_ShouldReturnFilePath()
        {
            // Arrange
            var url = "http://example.com/log.txt";
            var minhaCDNLog = "original content";
            var transformedLog = "transformed content";

            _transformerMock.Setup(x => x.Transform(minhaCDNLog))
                .Returns(transformedLog);

            // Act
            var result = await _service.TransformLogFromRequest(url, "file");

            // Assert
            Assert.Contains("ConvertedLogs\\log_", result);
            Assert.EndsWith(".txt", result);
            _repositoryMock.Verify(x => x.Save(It.IsAny<EntityLog>()), Times.Once);
        }

        [Fact]
        public async Task TransformLogFromRequest_WithResponseOutput_ShouldReturnTransformedContent()
        {
            // Arrange
            var url = "http://example.com/log.txt";
            var minhaCDNLog = "original content";
            var transformedLog = "transformed content";

            _transformerMock.Setup(x => x.Transform(minhaCDNLog))
                .Returns(transformedLog);

            // Act
            var result = await _service.TransformLogFromRequest(url, "response");

            // Assert
            Assert.Equal(transformedLog, result);
            _repositoryMock.Verify(x => x.Save(It.IsAny<EntityLog>()), Times.Once);
        }

        [Fact]
        public async Task TransformLogSavedById_WithFileOutput_ShouldReturnFilePath()
        {
            // Arrange
            var id = Guid.NewGuid();
            var originalLog = new EntityLog
            {
                Id = id,
                MinhaCDNLog = "original content"
            };
            var transformedContent = "transformed content";

            _repositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(originalLog);
            _transformerMock.Setup(x => x.Transform(originalLog.MinhaCDNLog))
                .Returns(transformedContent);

            // Act
            var result = await _service.TransformLogSavedById(id, "file");

            // Assert
            Assert.Contains("ConvertedLogs\\log_", result);
            Assert.EndsWith(".txt", result);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<EntityLog>()), Times.Once);
        }

        [Theory]
        [InlineData("original", null, "original")]
        [InlineData("original", "transformed", "original\n\ntransformed")]
        public void CombineLogs_ShouldCombineLogsCorrectly(string minhaCDNLog, string agoraLog, string expected)
        {
            // Act
            var result = _service.InvokePrivateMethod("CombineLogs",
                minhaCDNLog, agoraLog) as string;

            // Assert
            Assert.Equal(expected.Replace("\n", Environment.NewLine), result);
        }
    }
}
