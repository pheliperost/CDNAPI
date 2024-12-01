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

        public async Task<EntityLog> GetTransformedLogByIdAsync(Guid id)
        {
            var log = await _entityLogRepository.GetByIdAsync(id);
            return !string.IsNullOrEmpty(log.AgoraLog) ? log : null;
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
             var id =await _entityLogRepository.Save(log);


            return await _entityLogRepository.GetById(id);
        }


        public async Task<EntityLog> TransformLogAsync(string input, string inputType, string outputFormat)
        {
            string minhaCDNLog;
            if (inputType == "url")
            {
                using (var client = new HttpClient())
                {
                    minhaCDNLog = await client.GetStringAsync(input);
                }
            }
            else
            {
                var entitylog = await _entityLogRepository.GetByIdAsync(new Guid(input));
                minhaCDNLog = entitylog.MinhaCDNLog;
            }

            var agoraFormat = _logTransformer.Transform(minhaCDNLog);
            var log = new EntityLog
            {
                MinhaCDNLog = minhaCDNLog,
                AgoraLog = agoraFormat,
                URL = inputType == "url" ? input : null,
                OutputFormat = outputFormat,
                CreatedAt = DateTime.UtcNow
            };

            if (outputFormat == "file")
            {
                log.FilePath = await SaveToFileAsync(agoraFormat);
            }

            await _entityLogRepository.Save(log);
            return log;
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

        public void Dispose()
        {
            _entityLogRepository?.Dispose();
        }
    }
}
