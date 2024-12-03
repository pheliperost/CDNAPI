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
    public class FileUtilsServiceTests
    {
        private readonly EntityLogFixture _entityLogFixture;
        private readonly EntityLogService _entityLogService;
        
        public FileUtilsServiceTests(EntityLogFixture entityLogFixture)
        {
            _entityLogFixture = entityLogFixture;
            _entityLogService = _entityLogFixture.GetService();
        }

    }
}
