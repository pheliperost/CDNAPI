using CDNAPI.Interfaces;
using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using CDNAPI.Utils;
using CDNAPI.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace CDNAPI.Services
{
    public class EntityLogService : BaseService, IEntityLogService
    {
        IEntityLogRepository _entityLogRepository;
        IFileUtilsService _fileUtilsService;
        

        public EntityLogService(IEntityLogRepository entityLogRepository,
                                IFileUtilsService fileUtils)
        {
            _entityLogRepository = entityLogRepository;
            _fileUtilsService = fileUtils;
        }
        

        public async Task<EntityLog> GetSavedLogById(Guid id)
        {
            return await _entityLogRepository.GetById(id);
        }
        
        public async Task<String> GetTransformedLogById(Guid id)
        {
            var log = await _entityLogRepository.GetById(id);

            if (log == null)
                throw new InvalidOperationException($"Registro não encontrado.");

            return log.AgoraLog;
        }
        
        public async Task<String> GetOriginalAndTransformedLogById(Guid id)
        {
            var log = await _entityLogRepository.GetById(id);
            return CombineLogs(log.MinhaCDNLog, log.AgoraLog);
        }

        public async Task<string> TransformLogFromRequest(string url, string outputFormat)
        {
            ValidateInput(url, outputFormat);

            var minhaCDNLog = await _fileUtilsService.FetchLogAsync(url);
            var agoraFormat = LogTransformer.Transform(minhaCDNLog);

            var entitylog = CreateEntityLog(url, minhaCDNLog, agoraFormat);

            string result = await _fileUtilsService.ProcessOutputFormat(outputFormat, agoraFormat, entitylog);

            await AddEntityLog(entitylog);

            return result;
        }

        public async Task<String> TransformLogSavedById(Guid id, string outputFormat)
        {
            var entitylog = await _entityLogRepository.GetById(id);

            var agoraLog = LogTransformer.Transform(entitylog.MinhaCDNLog);

            entitylog.AgoraLog = agoraLog;

            string result = await _fileUtilsService.ProcessOutputFormat(outputFormat, agoraLog, entitylog);

            await UpdateEntityLog(entitylog);

            return result;
        }

        public async Task<EntityLog> AddEntityLog(EntityLog newEntityLog)
        {
            if (!ExecuteValidation(new EntityLogValidation(), newEntityLog))
            {
                throw new ValidationException("Não foi possível inserir, pois, há dados inválidos.");
            }

            var entityLogAdded = await _entityLogRepository.Save(newEntityLog);
            return entityLogAdded;
        }

        public async Task<EntityLog> UpdateEntityLog(EntityLog newEntityLog)
        {
            if (!ExecuteValidation(new EntityLogValidation(), newEntityLog))
            {
                throw new ValidationException("Não foi possível inserir, pois, há dados inválidos.");
            }

            var entityLogAdded = await _entityLogRepository.UpdateAsync(newEntityLog);
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
