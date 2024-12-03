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

        public EntityLog GenerateValidSavedEntityLog()
        {
            var entiLogs = new Faker<EntityLog>()
               .RuleFor(c => c.Id, f => f.Random.Guid())
               .RuleFor(c => c.MinhaCDNLog, f => f.Random.AlphaNumeric(10))
               .RuleFor(c => c.AgoraLog, f => string.Empty)
               .RuleFor(c => c.CreatedAt, f => f.Date.Past(2))
               .RuleFor(c => c.URL, f => f.Internet.Url() + "/"+f.Random.AlphaNumeric(5)+".txt")
               .RuleFor(c => c.FilePath, f => string.Empty);

            return entiLogs;
        }

        public EntityLog GenerateInvalidSavedEntityLog()
        {
            var entiLog = new EntityLog();
            return entiLog;
        }
        
        public EntityLog GenerateValidEntityLog()
        {
            var entiLogs = new Faker<EntityLog>()
               .RuleFor(c => c.Id, f => f.Random.Guid())
               .RuleFor(c => c.MinhaCDNLog, f => f.Random.AlphaNumeric(10))
               .RuleFor(c => c.AgoraLog, f => f.Random.AlphaNumeric(10))
               .RuleFor(c => c.CreatedAt, f => f.Date.Past(2))
               .RuleFor(c => c.URL, f => f.Internet.Url() + "/" + f.Random.AlphaNumeric(5) + ".txt")
               .RuleFor(c => c.FilePath, f => f.System.FilePath()+f.Date.Random.ToString()+".txt");

            return entiLogs;
        }

        public EntityLog GenerateInvalidEntityLog()
        {
            var entiLogs = new Faker<EntityLog>()
               .RuleFor(c => c.Id, f => f.Random.Guid())
               .RuleFor(c => c.MinhaCDNLog, string.Empty)
               .RuleFor(c => c.AgoraLog,  string.Empty)
               .RuleFor(c => c.CreatedAt, DateTime.MinValue)
               .RuleFor(c => c.URL,string.Empty)
               .RuleFor(c => c.FilePath, string.Empty);

            return entiLogs;
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
