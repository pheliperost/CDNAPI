using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Interfaces
{
    public interface IEntityLogService : IDisposable
    {
        Task<EntityLog> TransformLogAsync(string input, string inputType, string outputFormat);
        Task<IEnumerable<EntityLog>> GetSavedLogsAsync();
        Task<IEnumerable<EntityLog>> GetTransformedLogsAsync();
        Task<EntityLog> GetSavedLogByIdAsync(Guid id);
        Task<EntityLog> GetTransformedLogByIdAsync(Guid id);
        Task<Guid> SaveLogAsync(string content);
    }
}
