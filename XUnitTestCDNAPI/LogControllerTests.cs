//using CDNAPI.Services;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace XUnitTestCDNAPI
//{
//    public class LogControllerTests
//    {
//        private readonly Mock<ILogService> _serviceMock;
//        private readonly EntityLogService _entitylogService;

//        public LogControllerTests()
//        {
//            _serviceMock = new Mock<ILogService>();
//            _controller = new LogsController(_serviceMock.Object);
//        }

//        [Fact]
//        public async Task TransformLog_ValidRequest_ReturnsOkResult()
//        {
//            // Arrange
//            var request = new LogTransformRequest
//            {
//                Input = "http://example.com/log.txt",
//                InputType = "url",
//                OutputFormat = "response"
//            };

//            _serviceMock.Setup(x => x.TransformLogAsync(request))
//                .ReturnsAsync("transformed content");

//            // Act
//            var result = await _controller.TransformLog(request);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.NotNull(okResult.Value);
//        }

//        [Fact]
//        public async Task GetSavedLogById_ExistingId_ReturnsOkResult()
//        {
//            // Arrange
//            var id = Guid.NewGuid();
//            var log = new Log { Id = id };
//            _serviceMock.Setup(x => x.GetSavedLogByIdAsync(id))
//                .ReturnsAsync(log);

//            // Act
//            var result = await _controller.GetSavedLogById(id);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(log, okResult.Value);
//        }
//    }
//}
