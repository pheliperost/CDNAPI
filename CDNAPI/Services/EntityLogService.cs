using CDNAPI.Interfaces;
using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using CDNAPI.Utils;

namespace CDNAPI.Services
{
    public class EntityLogService : BaseService, IEntityLogService
    {
        IEntityLogRepository _entityLogRepository;
        ILogTransformer _logTransformer;
        IFileUtilsService _fileUtilsService;
        

        public EntityLogService(IEntityLogRepository entityLogRepository,
                                ILogTransformer logTransformer,
                                IFileUtilsService fileUtils)
        {
            _entityLogRepository = entityLogRepository;
            _logTransformer = logTransformer;
            _fileUtilsService = fileUtils;
        }
        

        public Task<EntityLog> GetSavedLogByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EntityLog>> GetSavedLogsAsync()
        {
            return await _entityLogRepository.GetAllAsync();
        }

        public async Task<EntityLog> GetSavedLogByIdAsync(Guid id)
        {
            return await _entityLogRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<String>> GetTransformedLogsAsync()
        {
            var logs = await _entityLogRepository.GetAllAsync();
            
                logs = logs.Where(l => !string.IsNullOrEmpty(l.AgoraLog));

            return logs.Select(p => p.AgoraLog).ToList();
        }

        public async Task<String> GetTransformedLogByIdAsync(Guid id)
        {
            var log = await _entityLogRepository.GetByIdAsync(id);
            return log.AgoraLog;
        }

        public async Task<String> GetOriginalAndTransformedLogById(Guid id)
        {
            var log = await _entityLogRepository.GetByIdAsync(id);
            return CombineLogs(log.MinhaCDNLog, log.AgoraLog);
        }

        public async Task<string> TransformLogFromRequest(string url, string outputFormat)
        {
            ValidateInput(url, outputFormat);

            var minhaCDNLog = await _fileUtilsService.FetchLogAsync(url);
            var agoraFormat = _logTransformer.Transform(minhaCDNLog);

            var entitylog = CreateEntityLog(url, minhaCDNLog, agoraFormat);

            string result = await _fileUtilsService.ProcessOutputFormat(outputFormat, agoraFormat, entitylog);

            await AddEntityLog(entitylog);

            return result;
        }

        public async Task<EntityLog> AddEntityLog(EntityLog newEntityLog)
        {
            var entityLogAdded = await _entityLogRepository.Save(newEntityLog);
            return entityLogAdded;
        }

        public EntityLog CreateEntityLog(string url, string minhaCDNLog, string agoraFormat)
        {
            return new EntityLog
            {
                MinhaCDNLog = minhaCDNLog,
                AgoraLog = agoraFormat,
                URL = url,
                CreatedAt = DateTime.UtcNow
            };
        }

        public async Task<String> TransformLogSavedById(Guid id, string outputFormat)
        {
            string result = "";

            var entitylog = await _entityLogRepository.GetByIdAsync(id);

            var agoraLog = _logTransformer.Transform(entitylog.MinhaCDNLog);


            entitylog.AgoraLog = agoraLog;

            if (outputFormat == "file")
            {
                entitylog.FilePath = await _fileUtilsService.SaveToFileAsync(agoraLog);
                result = entitylog.FilePath;
            }

            if (outputFormat == "response")
            {
                result = entitylog.AgoraLog;
            }

            await _entityLogRepository.UpdateAsync(entitylog);

            return result;
        }

        private void ValidateInput(string url, string outputFormat)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL não pode ser vazia ou nula.", nameof(url));

            if (string.IsNullOrWhiteSpace(outputFormat))
                throw new ArgumentException("Formato de saída não pode ser vazio ou nulo.", nameof(outputFormat));
        }


        private string CombineLogs(string minhaCDNLog, string agoraLog)
        {
            if (string.IsNullOrEmpty(agoraLog))
            {
                return minhaCDNLog;
            }
            return $"{minhaCDNLog}{Environment.NewLine}{Environment.NewLine}{agoraLog}";
        }

        public void Dispose()
        {
            _entityLogRepository?.Dispose();
        }

    }
}
