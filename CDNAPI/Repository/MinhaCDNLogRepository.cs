using CDNAPI.Interfaces;
using CDNAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Repository
{
    public class MinhaCDNLogRepository : Repository<MinhaCDNLog>, IMinhaCDNLogRepository
    {
        public MinhaCDNLogRepository(ApiDbContext apiDbContext) : base(apiDbContext){}

        public async Task<MinhaCDNLog> GetMinhaCDNLog(Guid Id)
        {
            return await _apiDbContext.MinhaCDNLogs.Where(p => p.Id == Id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
