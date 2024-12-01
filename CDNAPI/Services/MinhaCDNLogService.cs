using CDNAPI.Interfaces;
using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Services
{
    public class MinhaCDNLogService : BaseService, IMinhaCDNLogService
    {
        IMinhaCDNLogRepository _minhaCDNRepository;

        public MinhaCDNLogService(IMinhaCDNLogRepository minhaCDNRepository)
        {
            _minhaCDNRepository = minhaCDNRepository;
        }
        public async Task Add(MinhaCDNLog minhaCDNLog)
        {
            await _minhaCDNRepository.Add(minhaCDNLog);
        }
        public async Task<MinhaCDNLog> GetById(Guid id)
        {
           return await _minhaCDNRepository.GetMinhaCDNLog(id);
        }

        public void Dispose()
        {
            _minhaCDNRepository?.Dispose();
        }

        
    }
}
