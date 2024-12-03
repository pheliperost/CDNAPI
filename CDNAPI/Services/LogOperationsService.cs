using CDNAPI.Interfaces;
using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CDNAPI.Services
{
    public class LogOperationsService : BaseService, ILogOperationsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public LogOperationsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<string> FetchLogAsync(string url)
        {
            if (_httpClientFactory == null)
            {
                throw new InvalidOperationException("EntityLogUtils não foi inicializado.");
            }

            var client = _httpClientFactory.CreateClient();
            return await client.GetStringAsync(url);
        }

        public async Task<string> SaveToFileAsync(string content)
        {
            //passar essa pasta para um arquivo de configuração para facilitar a mudança de diretório se necessário.
            string directory = "ConvertedLogs";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"log_{timestamp}.txt";
            string filePath = Path.Combine(directory, fileName);

            await File.WriteAllTextAsync(filePath, content);

            return filePath;
        }
        public async Task<string> ProcessOutputFormat(string outputFormat, string agoraFormat, EntityLog log)
        {
            switch (outputFormat.ToLower())
            {
                case "file":
                    log.FilePath = await SaveToFileAsync(agoraFormat);
                    return log.FilePath;
                case "response":
                    return log.AgoraLog;
                default:
                    throw new ArgumentException("Formato de saída inválido.", nameof(outputFormat));
            }
        }
        public string TransformLog(string input)
        {
            var lines = input.Split('\n');
            var output = new StringBuilder();

            output.AppendLine("#Version: 1.0");
            output.AppendLine($"#Date: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            output.AppendLine("#Fields: provider http-method status-code uri-path time-taken response-size cache-status");
            output.AppendLine();

            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length != 5) continue;

                var responseSize = parts[0];
                var statusCode = parts[1];
                var cacheStatus = parts[2];
                var httpMethod = parts[3].Split(' ')[0].Trim('"');
                var uriPath = parts[3].Split(' ')[1];
                var timeTaken = Math.Round(double.Parse(parts[4]));

                if (cacheStatus == "INVALIDATE")
                    cacheStatus = "REFRESH_HIT";

                output.AppendLine($"\"MINHA CDN\" {httpMethod} {statusCode} {uriPath} {timeTaken} {responseSize} {cacheStatus}");
            }

            return output.ToString();
        }

        public string AppendLogs(string minhaCDNLog, string agoraLog)
        {
            if (string.IsNullOrEmpty(agoraLog))
            {
                return minhaCDNLog;
            }
            return $"{minhaCDNLog}{Environment.NewLine}{Environment.NewLine}{agoraLog}";
        }
        public void Dispose()
        {
        }

    }
}
