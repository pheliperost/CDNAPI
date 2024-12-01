using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Interfaces
{
    public interface IMinhaCDNLogRepository : IRepository<MinhaCDNLog>
    {
        Task<MinhaCDNLog> GetMinhaCDNLog(Guid Id);
    }
}
