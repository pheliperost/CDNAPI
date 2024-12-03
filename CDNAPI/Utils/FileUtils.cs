using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CDNAPI.Utils
{
    public static class FileUtils
    {
        private static IHttpClientFactory _httpClientFactory;
        public static void Initialize(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public static async Task<string> FetchLogAsync(string url)
        {
            if (_httpClientFactory == null)
            {
                throw new InvalidOperationException("EntityLogUtils não foi inicializado.");
            }

            var client = _httpClientFactory.CreateClient();
            return await client.GetStringAsync(url);
        }


        public static async Task<string> SaveToFileAsync(string content)
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


    }
}
