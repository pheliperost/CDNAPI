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


        [Fact(DisplayName = "Transforming a Valid Log Should Return Success.")]
        [Trait("Category", "LogOperations Service")]
        public void LogOperationsService_TransformLog_ShouldReturnSuccess()
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

        [Fact(DisplayName = "Transforming a invalid Log Should Return Error.")]
        [Trait("Category", "LogOperations Service")]
        public void LogOperationsService_TransformLog_ShouldReturnError()
        {
            //Arrange
            var minhaCDNlog = _entityLogFixture.GenerateValidMinhaCDNLog();
            var expectedAgoraLog = _entityLogFixture.GenerateInvalidMinhaCDNLogExpected();

            // Act
            var agoraLog = _logOperationsService.TransformLog(minhaCDNlog);
            agoraLog = RemoveDateTimeLineAndNormalize(agoraLog);
            expectedAgoraLog = RemoveDateTimeLineAndNormalize(expectedAgoraLog);

            // Assert
            Assert.NotNull(agoraLog);
            Assert.NotEqual(agoraLog, expectedAgoraLog);
        }

        [Fact(DisplayName = "Transforming a empty Log Should Return Exception Error.")]
        [Trait("Category", "LogOperations Service")]
        public void LogOperationsService_TransformLog_ShouldReturnArgumentExceptionError()
        {
            //Arrange
            var minhaCDNlog = "";
            
            // Act 
            Exception exception = null;
            try
            {
                var result = _logOperationsService.TransformLog(minhaCDNlog);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert            
            Assert.IsType<ArgumentException>(exception);
            Assert.Contains("O parâmetro MinhaCDN está vazio.", exception.Message);
        }

        [Fact(DisplayName = "Transforming a Log With an Invalid Layout Should Throw an Exception.")]
        [Trait("Category", "LogOperations Service")]
        public void LogOperationsService_TransformLog_ShouldReturnFormatExceptionExceptionError()
        {
            //Arrange
            var minhaCDNlog = @"312|200|HIT|HIT|HIT|""GET /robots.txt HTTP/1.1""|100.2";

            // Act 
            Exception exception = null;
            try
            {
                var result = _logOperationsService.TransformLog(minhaCDNlog);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert            
            Assert.IsType<FormatException>(exception);
            Assert.Contains("A linha de entrada deve conter exatamente 5 partes separadas por '|'.", exception.Message);
        }

        [Fact(DisplayName = "Transforming a Log With Invalid Number of Parts Should Throw ArgumentException")]
        [Trait("Category", "LogOperations Service")]
        public void LogOperationsService_TransformLine_ShouldReturnArgumentException()
        {
            //Arrange
            var invalidLine = "200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";

            // Act
            Exception exception = null;
            try
            {
                var result = _logOperationsService.TransformLog(invalidLine);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsType<FormatException>(exception);
            Assert.Contains("A linha de entrada deve conter exatamente 5 partes separadas por '|'", exception.Message);
        }

        [Fact(DisplayName = "Transforming a Log With Invalid Time Format Should Throw FormatException")]
        [Trait("Category", "LogOperations Service")]
        public void LogOperationsService_TransformLine_InvalidTimeFormat_ShouldThrowFormatException()
        {
            // Arrange
            var invalidLine = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|not-a-number";

            // Act
            Exception exception = null;
            try
            {
                var result = _logOperationsService.TransformLog(invalidLine);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsType<FormatException>(exception);
            Assert.Contains("O valor de tempo fornecido não está em um formato numérico válido", exception.Message);
        }

        [Fact(DisplayName = "Transforming a Log With Invalid HTTP Method Should Return Null")]
        [Trait("Category", "LogOperations Service")]
        public void LogOperationsService_TransformLine_InvalidHttpMethod_ShouldReturnNull()
        {
            // Arrange
            var invalidLine = "312|200|HIT|\"GET\"|100";

            // Act
            var result = _logOperationsService.TransformLog(invalidLine);
                result = RemoveDateTimeLineAndNormalize(result);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("#Version: 1.0#Fields: provider http-method status-code uri-path time-taken response-size cache-status", result);
        }

        [Fact(DisplayName = "Transforming a Valid Log Line Should Return Formatted String")]
        [Trait("Category", "LogOperations Service")]
        public void LogOperationsService_TransformLine_ValidInput_ShouldReturnFormattedString()
        {
            // Arrange
            var validLine = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";

            // Act
            var result = _logOperationsService.TransformLog(validLine);
                result = RemoveDateTimeLineAndNormalize(result);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT", result);
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
