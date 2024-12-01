﻿using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Interfaces
{
    public interface IEntityLogService : IDisposable
    {
        Task<String> TransformLogFromRequest(string input, string inputType, string outputFormat);
        Task<String> TransformLogSavedById(Guid id, string outputFormat);
        Task<IEnumerable<EntityLog>> GetSavedLogsAsync();
        Task<IEnumerable<String>> GetTransformedLogsAsync();
        Task<EntityLog> GetSavedLogByIdAsync(Guid id);
        Task<String> GetTransformedLogByIdAsync(Guid id);
        Task<String> GetOriginalAndTransformedLogById(Guid id);
        Task<EntityLog> SaveLogMinhaCDNFormat(string content);
    }
}
