using Bogus;
using CDNAPI.Models;
using CDNAPI.Services;
using CDNAPI.ViewModels;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestCDNAPI.Fixtures
{
    [CollectionDefinition(nameof(EntityLogCollection))]
    public class EntityLogCollection : ICollectionFixture<EntityLogFixture>, 
        ICollectionFixture<RequestLogFixture>
    { }
    public class EntityLogFixture : IDisposable
    {
        public EntityLogService _entityLogService;
        public RequestLogFixture _requestLogFixture;
        public AutoMocker Mocker;

        public IEnumerable<EntityLog> GenerarateAValidSavedEntityLog(int quantityToGenerate)
        {
            var entiLogs = new Faker<EntityLog>()
               .RuleFor(c => c.MinhaCDNLog, f => f.Random.AlphaNumeric(10))
               .RuleFor(c => c.AgoraLog, f => string.Empty)
               .RuleFor(c => c.CreatedAt, f => f.Date.Past(2))
               .RuleFor(c => c.URL, f => f.Internet.Url() + "/"+f.Random.AlphaNumeric(5)+".txt")
               .RuleFor(c => c.FilePath, f => string.Empty);

            return entiLogs.Generate(quantityToGenerate);
        }


        public IEnumerable<EntityLog> GenerarateValidEntityLog(int quantityToGenerate)
        {
            var entiLogs = new Faker<EntityLog>()
               .RuleFor(c => c.MinhaCDNLog, f => f.Random.AlphaNumeric(10))
               .RuleFor(c => c.AgoraLog, f => string.Empty)
               .RuleFor(c => c.CreatedAt, f => f.Date.Past(2))
               .RuleFor(c => c.URL, f => f.Internet.Url() + "/" + f.Random.AlphaNumeric(5) + ".txt")
               .RuleFor(c => c.FilePath, f => string.Empty);

            return entiLogs.Generate(quantityToGenerate);
        }

        public String GenerarateValidAgoraLog()
        {
            return new Faker().Random.AlphaNumeric(30);
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
