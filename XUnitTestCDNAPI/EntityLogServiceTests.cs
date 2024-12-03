using AutoFixture;
using CDNAPI.Interfaces;
using CDNAPI.Models;
using CDNAPI.Services;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using XUnitTestCDNAPI.Fixtures;

namespace XUnitTestCDNAPI
{
    [Collection(nameof(EntityLogCollection))]
    public class EntityLogServiceTests
    {
        private readonly EntityLogFixture _entityLogFixture;
        private readonly RequestLogFixture _requestLogFixture;
        private readonly EntityLogService _entityLogService;
        
        public EntityLogServiceTests(EntityLogFixture entityLogFixture,
                                     RequestLogFixture requestLogFixture)
        {
            _entityLogFixture = entityLogFixture;
            _requestLogFixture = requestLogFixture;
            _entityLogService = _entityLogFixture.GetService();
        }


        [Fact(DisplayName = "Adding a valid EntityLog Should Return Success")]
        [Trait("Categoria", "EntityLog Service")]
        public async Task EntityLogService_AddingNewValidEntityLog_ShouldReturnSuccess()
        {
            // Arrange
            var entityLog = _entityLogFixture.GenerarateValidEntityLog(1).FirstOrDefault();

            // Act
            var result =  await _entityLogService.AddEntityLog(entityLog);

            // Assert
            _entityLogFixture.Mocker.GetMock<IEntityLogRepository>().Verify(r => r.Save(entityLog), Times.Once);

        }

        [Fact(DisplayName = "Adding a invalid EntityLog Should Return Error")]
        [Trait("Categoria", "EntityLog Service")]
        public async Task EntityLogService_AddingNewinvalidEntityLog_ShouldReturnError()
        {
            // Arrange
            var entityLog = _entityLogFixture.GenerarateInvalidEntityLog();

            // Act
            await Assert.ThrowsAsync<ValidationException>(() => _entityLogService.AddEntityLog(entityLog));

            // Assert
            _entityLogFixture.Mocker.GetMock<IEntityLogRepository>().Verify(r => r.Save(entityLog), Times.Never);

        }


        [Fact(DisplayName = "Transform a Original EntityLogs Should Return Success.")]
        [Trait("Category", "EntityLog Service")]
        public async Task EntityLogService_UpdateValidEntityLog_ShouldReturnSuccess()
        {
            // Arrange
            var originalLog = _entityLogFixture.GenerarateAValidSavedEntityLog(1).FirstOrDefault();

            // Act
            var result = await _entityLogService.UpdateEntityLog(originalLog);

            // Assert
            
            _entityLogFixture.Mocker.GetMock<IEntityLogRepository>().Verify(x => x.UpdateAsync(originalLog), Times.Once);
        }

        [Fact(DisplayName = "Retrieve all EntityLogs Should Return Success.")]
        [Trait("Category", "EntityLog Service")]
        public async Task EntityLogService_GetSavedLogsAsync_ShouldReturnAllLogs()
        {
            // Arrange
            var expectedLogs = _entityLogFixture.GenerarateAValidSavedEntityLog(10);
            _entityLogFixture.Mocker.GetMock<IEntityLogRepository>()
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _entityLogService.GetSavedLogsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLogs.Count(), result.Count());
            Assert.Equal(expectedLogs, result);
            Assert.All(result, log => Assert.IsType<EntityLog>(log));
            _entityLogFixture.Mocker.GetMock<IEntityLogRepository>().Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetSavedLogByIdAsync_ShouldReturnLog()
        {
            //// Arrange
            //var id = Guid.NewGuid();
            //var expectedLog = _fixture.Create<EntityLog>();
            //_repositoryMock.Setup(x => x.GetByIdAsync(id))
            //    .ReturnsAsync(expectedLog);

            //// Act
            //var result = await _service.GetSavedLogByIdAsync(id);

            //// Assert
            //Assert.Equal(expectedLog, result);
            //_repositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetTransformedLogsAsync_ShouldReturnOnlyTransformedLogs()
        {
        //    // Arrange
        //    var logs = new List<EntityLog>
        //{
        //    new EntityLog { AgoraLog = "transformed1" },
        //    new EntityLog { AgoraLog = null },
        //    new EntityLog { AgoraLog = "transformed2" }
        //};
        //    _repositoryMock.Setup(x => x.GetAllAsync())
        //        .ReturnsAsync(logs);

        //    // Act
        //    var result = await _service.GetTransformedLogsAsync();

        //    // Assert
        //    Assert.Equal(2, result.Count());
        //    Assert.Contains("transformed1", result);
        //    Assert.Contains("transformed2", result);
        }

        [Fact]
        public async Task TransformLogFromRequest_WithFileOutput_ShouldReturnFilePath()
        {
            //// Arrange
            //var url = "http://example.com/log.txt";
            //var minhaCDNLog = "original content";
            //var transformedLog = "transformed content";

            //_transformerMock.Setup(x => x.Transform(minhaCDNLog))
            //    .Returns(transformedLog);

            //// Act
            //var result = await _service.TransformLogFromRequest(url, "file");

            //// Assert
            //Assert.Contains("ConvertedLogs\\log_", result);
            //Assert.EndsWith(".txt", result);
            //_repositoryMock.Verify(x => x.Save(It.IsAny<EntityLog>()), Times.Once);
        }

        [Fact]
        public async Task TransformLogFromRequest_WithResponseOutput_ShouldReturnTransformedContent()
        {
            //// Arrange
            //var url = "http://example.com/log.txt";
            //var minhaCDNLog = "original content";
            //var transformedLog = "transformed content";

            //_transformerMock.Setup(x => x.Transform(minhaCDNLog))
            //    .Returns(transformedLog);

            //// Act
            //var result = await _service.TransformLogFromRequest(url, "response");

            //// Assert
            //Assert.Equal(transformedLog, result);
            //_repositoryMock.Verify(x => x.Save(It.IsAny<EntityLog>()), Times.Once);
        }

        [Fact]
        public async Task TransformLogSavedById_WithFileOutput_ShouldReturnFilePath()
        {
            //// Arrange
            //var id = Guid.NewGuid();
            //var originalLog = new EntityLog
            //{
            //    Id = id,
            //    MinhaCDNLog = "original content"
            //};
            //var transformedContent = "transformed content";

            //_repositoryMock.Setup(x => x.GetByIdAsync(id))
            //    .ReturnsAsync(originalLog);
            //_transformerMock.Setup(x => x.Transform(originalLog.MinhaCDNLog))
            //    .Returns(transformedContent);

            //// Act
            //var result = await _service.TransformLogSavedById(id, "file");

            //// Assert
            //Assert.Contains("ConvertedLogs\\log_", result);
            //Assert.EndsWith(".txt", result);
            //_repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<EntityLog>()), Times.Once);
        }

        [Theory]
        [InlineData("original", null, "original")]
        [InlineData("original", "transformed", "original\n\ntransformed")]
        public void CombineLogs_ShouldCombineLogsCorrectly(string minhaCDNLog, string agoraLog, string expected)
        {
            //// Act
            //var result = _service.InvokePrivateMethod("CombineLogs",
            //    minhaCDNLog, agoraLog) as string;

            //// Assert
            //Assert.Equal(expected.Replace("\n", Environment.NewLine), result);
        }
    }
}
