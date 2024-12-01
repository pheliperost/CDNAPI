using CDNAPI.Interfaces;
using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace CDNAPI.Services
{
    public class EntityLogService : BaseService, IEntityLogService
    {
        IEntityLogRepository _entityLogRepository;
        ILogTransformer _logTransformer;

        public EntityLogService(IEntityLogRepository entityLogRepository,
                                ILogTransformer logTransformer)
        {
            _entityLogRepository = entityLogRepository;
            _logTransformer = logTransformer;
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

        public async Task<EntityLog> SaveLogMinhaCDNFormat(string content)
        {
            string MinhaCDNLogContent;
            using (var client = new HttpClient())
            {
                MinhaCDNLogContent = await client.GetStringAsync(content);
            }
            var log = new EntityLog
            {
                MinhaCDNLog = MinhaCDNLogContent,
                URL = content,
                CreatedAt = DateTime.UtcNow
            };

            return await _entityLogRepository.Save(log);
        }


        public async Task<String> TransformLogFromRequest(string url, string inputType, string outputFormat)
        {
            string minhaCDNLog, result = "";

            using (var client = new HttpClient())
            {
                minhaCDNLog = await client.GetStringAsync(url);
            }
            

            var agoraFormat = _logTransformer.Transform(minhaCDNLog);
            var log = new EntityLog
            {
                MinhaCDNLog = minhaCDNLog,
                AgoraLog = agoraFormat,
                URL = inputType == "url" ? url : null,
                OutputFormat = outputFormat,
                CreatedAt = DateTime.UtcNow
            };

            if (outputFormat == "file")
            {
                log.FilePath = await SaveToFileAsync(agoraFormat);
                result = log.FilePath;
            }

            if (outputFormat == "response")
            {
                result = log.AgoraLog;
            }


            await _entityLogRepository.Save(log);
            return result;
        }

        public async Task<String> TransformLogSavedById(Guid id, string outputFormat)
        {
            string result = "";

            var entitylog = await _entityLogRepository.GetByIdAsync(id);

            var agoraLog = _logTransformer.Transform(entitylog.MinhaCDNLog);


            entitylog.AgoraLog = agoraLog;

            await _entityLogRepository.UpdateAsync(entitylog);

            if (outputFormat == "file")
            {
                entitylog.FilePath = await SaveToFileAsync(agoraLog);
                result = entitylog.FilePath;
            }

            if (outputFormat == "response")
            {
                result = entitylog.AgoraLog;
            }

            return result;
        }

        private async Task<string> SaveToFileAsync(string content)
        {
            string directory = "ConvertedLogs";

            // Cria o diretório se não existir
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Gera um nome único para o arquivo usando timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"log_{timestamp}.txt";
            string filePath = Path.Combine(directory, fileName);

            // Salva o conteúdo no arquivo de forma assíncrona
            await File.WriteAllTextAsync(filePath, content);

            return filePath;
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
