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
        public string AppendLogs(string minhaCDNLog, string agoraLog)
        {
            if (string.IsNullOrEmpty(agoraLog))
            {
                return minhaCDNLog;
            }
            return $"{minhaCDNLog}{Environment.NewLine}{Environment.NewLine}{agoraLog}";
        }

        public string TransformLog(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("O campo MinhaCDN está vazio.", nameof(input));
            }

            var output = new StringBuilder();
            AppendHeader(output);

            var lines = input.Split('\n');
            foreach (var line in lines)
            {
                try
                {
                    var transformedLine = TransformLine(line);
                    if (!string.IsNullOrEmpty(transformedLine))
                    {
                        output.AppendLine(transformedLine);
                    }
                }
                catch (Exception ex)
                {
                    throw new FormatException($"Erro ao processar a linha: {line}. Erro: {ex.Message}");
                }
            }

            return output.ToString();
        }
        private void AppendHeader(StringBuilder output)
        {
            output.AppendLine("#Version: 1.0");
            output.AppendLine($"#Date: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            output.AppendLine("#Fields: provider http-method status-code uri-path time-taken response-size cache-status");
            output.AppendLine();
        }

        private string TransformLine(string line)
        {
            var parts = line.Split('|');
            if (parts.Length != 5)
            {
                return null;
            }

            var responseSize = parts[0];
            var statusCode = parts[1];
            var cacheStatus = NormalizeCacheStatus(parts[2]);
            var httpMethodParts = parts[3].Split(' ');
            if (httpMethodParts.Length < 2)
            {
                return null;
            }
            var httpMethod = httpMethodParts[0].Trim('"');
            var uriPath = httpMethodParts[1];

            if (!double.TryParse(parts[4], out double timeTakenRaw))
            {
                return null;
            }
            var timeTaken = Math.Round(timeTakenRaw);

            return $"\"MINHA CDN\" {httpMethod} {statusCode} {uriPath} {timeTaken} {responseSize} {cacheStatus}";
        }

        private string NormalizeCacheStatus(string cacheStatus)
        {
            return cacheStatus == "INVALIDATE" ? "REFRESH_HIT" : cacheStatus;
        }

        public void Dispose()
        {
        }

    }
}
