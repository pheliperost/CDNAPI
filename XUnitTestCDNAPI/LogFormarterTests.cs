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
    public class LogFormaterTests
    {
        private readonly EntityLogFixture _entityLogFixture;
        private readonly LogOperationsService _logOperationsService;
        
        public LogFormaterTests(EntityLogFixture entityLogFixture)
        {
            _entityLogFixture = entityLogFixture;
            _logOperationsService = _entityLogFixture.GetLogOperationsService();
        }


        [Fact(DisplayName = "Append MinhaCDN and Agora Log Should Return Success.")]
        [Trait("Category", "EntityLog Service")]
        public void EntityLogService_AppendLogs_ShouldAppendBothLogsCorrectly()
        {
            //Arrange
            var minhaCDNlog = _entityLogFixture.GenerateValidEntityLog().MinhaCDNLog;
            var agoraLog = _entityLogFixture.GenerateValidEntityLog().AgoraLog;

            var espectedResult = $"{minhaCDNlog}{Environment.NewLine}{Environment.NewLine}{agoraLog}";

            // Act
            var result = _logOperationsService.AppendLogs(minhaCDNlog, agoraLog);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(espectedResult, result);
        }

        [Fact(DisplayName = "ProcessOutputFormat Should Return FilePath With Success.")]
        [Trait("Category", "EntityLog Service")]
        public async Task EntityLogService_ProcessOutputFormat_ShouldReturnFilePath()
        {
            //Arrange
            var validEntity = _entityLogFixture.GenerateValidEntityLog();
            var minhaCDNlog = validEntity.MinhaCDNLog;
            var agoraLog = validEntity.AgoraLog;


            var outputFormat = "file";

            var fileUtilsMock = new Mock<ILogOperationsService>();
            fileUtilsMock.Setup(f => f.SaveToFileAsync(agoraLog)).ReturnsAsync(validEntity.FilePath);

            // Act
            var result = await _logOperationsService.ProcessOutputFormat(outputFormat, agoraLog, validEntity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(validEntity.FilePath, result);
        }

        [Fact(DisplayName = "ProcessOutputFormat Should Return Response With Success.")]
        [Trait("Category", "EntityLog Service")]
        public async Task EntityLogService_ProcessOutputFormat_ShouldReturnResponse()
        {
            //Arrange
            var validEntity = _entityLogFixture.GenerateValidEntityLog();
            var minhaCDNlog = validEntity.MinhaCDNLog;
            var agoraLog = validEntity.AgoraLog;


            var outputFormat = "response";

            var fileUtilsMock = new Mock<ILogOperationsService>();
            fileUtilsMock.Setup(f => f.SaveToFileAsync(agoraLog)).ReturnsAsync(agoraLog);

            // Act
            var result = await _logOperationsService.ProcessOutputFormat(outputFormat, agoraLog, validEntity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(agoraLog, result);
        }

        [Fact(DisplayName = "ProcessOutputFormat Should Return Invalid Option With Error.")]
        [Trait("Category", "EntityLog Service")]
        public async Task EntityLogService_ProcessOutputFormat_ShouldReturnError()
        {
            //Arrange
            var validEntity = _entityLogFixture.GenerateValidEntityLog();
            var minhaCDNlog = validEntity.MinhaCDNLog;
            var agoraLog = validEntity.AgoraLog;


            var outputFormat = "opcaoinvalida";

            var fileUtilsMock = new Mock<ILogOperationsService>();
            fileUtilsMock.Setup(f => f.SaveToFileAsync(agoraLog)).ReturnsAsync(agoraLog);

            // Act
            Exception exception = null;
            try
            {
                var result = await _logOperationsService.ProcessOutputFormat(outputFormat, agoraLog, validEntity);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            
            // Assert
            Assert.IsType<ArgumentException>(exception);
            Assert.Contains("Formato de saída inválido.", exception.Message);
        }

    }
}
