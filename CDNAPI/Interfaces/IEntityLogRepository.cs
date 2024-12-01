using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Interfaces
{
    public interface IEntityLogRepository : IRepository<EntityLog>
    {
        Task<EntityLog> GetByIdAsync(Guid id);
        Task<IEnumerable<EntityLog>> GetAllAsync();
        Task<EntityLog> Save(EntityLog entitylog);
        Task<EntityLog> UpdateAsync(EntityLog entitylog);
    }
}
