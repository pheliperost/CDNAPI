using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Interfaces
{
    public interface ILogOperationsService : IDisposable
    {
        Task<string> FetchLogAsync(string url);
        Task<string> SaveToFileAsync(string content);
        Task<string> ProcessOutputFormat(string outputFormat, string agoraFormat, EntityLog log);
        string AppendLogs(string minhaCDNLog, string agoraLog);
        string TransformLog(string input);
    }
}
