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
            var entityLog = _entityLogFixture.GenerateValidSavedEntityLog(1).FirstOrDefault();

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
            var entityLog = _entityLogFixture.GenerateInvalidEntityLog();

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
            var originalLog = _entityLogFixture.GenerateValidSavedEntityLog(1).FirstOrDefault();

            // Act
            var result = await _entityLogService.UpdateEntityLog(originalLog);

            // Assert            
            _entityLogFixture.Mocker.GetMock<IEntityLogRepository>().Verify(x => x.UpdateAsync(originalLog), Times.Once);
        }

        [Fact(DisplayName = "Transform a Invalid Original EntityLogs Should Return Error.")]
        [Trait("Category", "EntityLog Service")]
        public async Task EntityLogService_UpdateAInvalidEntityLog_ShouldReturnError()
        {
            // Arrange
            var originalLog = _entityLogFixture.GenerateInvalidSavedEntityLog();

            // Act 
            Exception exception = null;
            try
            {
                await _entityLogService.UpdateEntityLog(originalLog);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert            
            Assert.IsType<ValidationException>(exception);
            _entityLogFixture.Mocker.GetMock<IEntityLogRepository>().Verify(x => x.UpdateAsync(originalLog), Times.Never);
        }

        [Fact(DisplayName = "Get a Existing Original EntityLogs Should Return Success.")]
        [Trait("Category", "EntityLog Service")]
        public async Task GetSavedLogById_ShouldReturnLog()
        {
            // Arrange
            var entityLog = _entityLogFixture.GenerateValidEntityLog(1).FirstOrDefault();

            _entityLogFixture.Mocker
                .GetMock<IEntityLogService>()
                .Setup(c => c.GetSavedLogById(entityLog.Id))
            .ReturnsAsync(entityLog);

            // Act
            var result = await _entityLogService.GetSavedLogById(entityLog.Id);

            // Assert
            _entityLogFixture.Mocker.GetMock<IEntityLogRepository>().Verify(x => x.GetById(entityLog.Id), Times.Never);
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

            //_repositoryMock.Setup(x => x.GetById(id))
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
