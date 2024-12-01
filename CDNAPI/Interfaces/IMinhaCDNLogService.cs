using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Interfaces
{
    public interface IMinhaCDNLogService : IDisposable
    {
        Task Add(MinhaCDNLog minhaCDNLog);
        Task <MinhaCDNLog>GetById(Guid id);
    }
}
