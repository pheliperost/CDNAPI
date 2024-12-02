using CDNAPI.Interfaces;
using CDNAPI.Models;
using CDNAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CDNAPI.Services
{
    public class FileUtilsService : BaseService, IFileUtilsService
    {
        public FileUtilsService(IHttpClientFactory httpClientFactory)
        {
            FileUtils.Initialize(httpClientFactory);
        }
        
        public Task<string> FetchLogAsync(string url)
        {
            return FileUtils.FetchLogAsync(url);
        }

        public Task<string> SaveToFileAsync(string content)
        {
            return FileUtils.SaveToFileAsync(content);
        }
        public Task<string> ProcessOutputFormat(string outputFormat, string agoraFormat, EntityLog log)
        {
            return FileUtils.ProcessOutputFormat(outputFormat, agoraFormat, log);
        }
        public void Dispose()
        {
        }

    }
}
