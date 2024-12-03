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
    public class LogOperationsTests
    {
        private readonly EntityLogFixture _entityLogFixture;
        private readonly LogOperationsService _logOperationsService;
        
        public LogOperationsTests(EntityLogFixture entityLogFixture)
        {
            _entityLogFixture = entityLogFixture;
            _logOperationsService = _entityLogFixture.GetLogOperationsService();
        }


        [Fact(DisplayName = "Append MinhaCDN and Agora Log Should Return Success.")]
        [Trait("Category", "LogOperations Service")]
        public void LogOperationsService_AppendLogs_ShouldAppendBothLogsCorrectly()
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
        [Trait("Category", "LogOperations Service")]
        public async Task LogOperationsService_ProcessOutputFormat_ShouldReturnFilePath()
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
        [Trait("Category", "LogOperations Service")]
        public async Task LogOperationsService_ProcessOutputFormat_ShouldReturnResponse()
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
        [Trait("Category", "LogOperations Service")]
        public async Task LogOperationsService_ProcessOutputFormat_ShouldReturnError()
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


        [Fact(DisplayName = "Transforming a Valid Log Should Return Return With Success.")]
        [Trait("Category", "LogOperations Service")]
        public void LogOperationsService_TransformLog_ShouldReturnWithSuccess()
        {
            //Arrange
            var minhaCDNlog = _entityLogFixture.GenerateValidMinhaCDNLog();
            var expectedAgoraLog = _entityLogFixture.GenerateValidMinhaCDNLogExpected(); 

            // Act
            var agoraLog =  _logOperationsService.TransformLog(minhaCDNlog);
            agoraLog = RemoveDateTimeLineAndNormalize(agoraLog);
            expectedAgoraLog = RemoveDateTimeLineAndNormalize(expectedAgoraLog);

            // Assert
            Assert.NotNull(agoraLog);

            Assert.Equal(agoraLog, expectedAgoraLog);
        }


        private static string RemoveDateTimeLineAndNormalize(string logText)
        {
            if (string.IsNullOrEmpty(logText))
            {
                return string.Empty;
            }

            var lines = logText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var filteredLines = RemoveDateTimeLine(lines);
            var joinedText = string.Join(Environment.NewLine, filteredLines);
            return NormalizeLineEndings(joinedText);
        }

        private static IEnumerable<string> RemoveDateTimeLine(string[] lines)
        {
            return lines.Where((line, index) => index != 1);
        }

        private static string NormalizeLineEndings(string text)
        {
            return text.Replace("\r", "").Replace("\n", "");
        }
    }
}
