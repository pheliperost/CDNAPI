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
            var entityLog = _entityLogFixture.GenerateValidSavedEntityLog();

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
        
        [Fact(DisplayName = "TransformLog a Original EntityLog Should Return Success.")]
        [Trait("Category", "EntityLog Service")]
        public async Task EntityLogService_UpdateValidEntityLog_ShouldReturnSuccess()
        {
            // Arrange
            var originalLog = _entityLogFixture.GenerateValidSavedEntityLog();

            // Act
            var result = await _entityLogService.UpdateEntityLog(originalLog);

            // Assert            
            _entityLogFixture.Mocker.GetMock<IEntityLogRepository>().Verify(x => x.UpdateAsync(originalLog), Times.Once);
        }

        [Fact(DisplayName = "TransformLog a Invalid Original EntityLog Should Return Error.")]
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

        [Fact(DisplayName = "Get a Existing Original EntityLog Should Return Success.")]
        [Trait("Category", "EntityLog Service")]
        public async Task EntityLogService_GetSavedLogById_ShouldReturnSuccess()
        {
            // Arrange
            var entityLog = _entityLogFixture.GenerateValidEntityLog();

            _entityLogFixture.Mocker
                .GetMock<IEntityLogService>()
                .Setup(c => c.GetSavedLogById(entityLog.Id))
            .ReturnsAsync(entityLog);

            // Act
            var result = await _entityLogService.GetSavedLogById(entityLog.Id);

            // Assert
            _entityLogFixture.Mocker.GetMock<IEntityLogRepository>().Verify(x => x.GetById(entityLog.Id), Times.Once);
        }

        [Fact(DisplayName = "Get a Existing Transformed EntityLog Should Return Success.")]
        [Trait("Category", "EntityLog Service")]
        public async Task EntityLogService_GetTransformedLogById_ShouldReturnSuccess()
        {
            // Arrange
            var entityLog = _entityLogFixture.GenerateValidEntityLog();

            _entityLogFixture.Mocker
                .GetMock<IEntityLogService>()
                .Setup(c => c.GetTransformedLogById(entityLog.Id))
            .ReturnsAsync(entityLog.AgoraLog);

            _entityLogFixture.Mocker
                .GetMock<IEntityLogRepository>()
                .Setup(c => c.GetById(entityLog.Id))
            .ReturnsAsync(entityLog);

            // Act
            var result = await _entityLogService.GetTransformedLogById(entityLog.Id);

            // Assert
            _entityLogFixture.Mocker.GetMock<IEntityLogRepository>().Verify(x => x.GetById(entityLog.Id), Times.Once);
        }

        [Fact(DisplayName = "Get a NonExisting Transformed EntityLog Should Return Error.")]
        [Trait("Category", "EntityLog Service")]
        public async Task EntityLogService_GetTransformedLogById_ShouldReturnError()
        {
            // Arrange
            var entityLog = _entityLogFixture.GenerateValidEntityLog();

            _entityLogFixture.Mocker
                .GetMock<IEntityLogService>()
                .Setup(c => c.GetTransformedLogById(entityLog.Id))
            .ReturnsAsync(entityLog.AgoraLog);
            
            // Act 
            Exception exception = null;
            try
            {
                var result = await _entityLogService.GetTransformedLogById(entityLog.Id);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert            
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Contains("Registro não encontrado.", exception.Message);
            _entityLogFixture.Mocker.GetMock<IEntityLogRepository>().Verify(x => x.GetById(entityLog.Id), Times.Once);
        }

    }
}
