﻿using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Interfaces
{
    public interface IEntityLogService : IDisposable
    {
        Task<String> TransformLogFromRequest(string url, string outputFormat);
        Task<String> TransformLogSavedById(Guid id, string outputFormat);
        Task<EntityLog> AddEntityLog(EntityLog newEntityLog);
        Task<EntityLog> GetSavedLogById(Guid id);
        Task<String> GetTransformedLogById(Guid id);
        Task<String> GetOriginalAndTransformedLogById(Guid id);
        EntityLog CreateEntityLog(string url, string minhaCDNLog, string agoraFormat);
    }
}
