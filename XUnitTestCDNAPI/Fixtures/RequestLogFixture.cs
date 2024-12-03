using Bogus;
using CDNAPI.Services;
using CDNAPI.ViewModels;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestCDNAPI.Fixtures
{
    [CollectionDefinition(nameof(RequestLogCollection))]
    public class RequestLogCollection : ICollectionFixture<RequestLogFixture>,
        ICollectionFixture<EntityLogFixture>
    { }
    public class RequestLogFixture : IDisposable
    {
        public EntityLogService _entityLogService;
        public AutoMocker Mocker;

        public RequestLogViewModel GenerarateAValidRequestLog()
        {
            var requestLogViewFaker = new Faker<RequestLogViewModel>()
               .RuleFor(c => c.URL, f => f.Internet.Url() + "/file.txt")
               .RuleFor(c => c.OutputFormat, f => f.PickRandom("file", "response"));

            return requestLogViewFaker;
        }

        public RequestLogViewModel GenerateInvalidRequestLog()
        {
            return new Faker<RequestLogViewModel>();
        }

        public EntityLogService GetService()
        {
            Mocker = new AutoMocker();
            _entityLogService = Mocker.CreateInstance<EntityLogService>();
            return _entityLogService;
        }
        public void Dispose()
        {
        }
    }
}
