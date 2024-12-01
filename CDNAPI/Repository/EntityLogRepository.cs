using CDNAPI.Interfaces;
using CDNAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Repository
{
    public class EntityLogRepository : Repository<EntityLog>, IEntityLogRepository
    {
        public EntityLogRepository(ApiDbContext apiDbContext) : base(apiDbContext){}
      

        public async Task<EntityLog> GetByIdAsync(Guid id)
        {
            return await _apiDbContext.EntityLogs.FindAsync(id);
        }

        public async Task<IEnumerable<EntityLog>> GetAllAsync()
        {
            return await _apiDbContext.EntityLogs.ToListAsync();
        }

        public async Task<Guid> Save(EntityLog entitylog)
        {
            _apiDbContext.EntityLogs.Add(entitylog);
            await _apiDbContext.SaveChangesAsync();
            return entitylog.Id;
        }

        public async Task<EntityLog> UpdateAsync(EntityLog entitylog)
        {
            _apiDbContext.EntityLogs.Update(entitylog);
            await _apiDbContext.SaveChangesAsync();
            return entitylog;
        }

        public async Task<EntityLog> GetEntityLog(Guid Id)
        {
            return await _apiDbContext.EntityLogs.Where(p => p.Id == Id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
