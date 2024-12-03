using CDNAPI.Interfaces;
using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDNAPI.Utils
{

    public static class LogFormater 
    {
        public static string TransformLog(string input)
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


        public static string CombineLogs(string minhaCDNLog, string agoraLog)
        {
            if (string.IsNullOrEmpty(agoraLog))
            {
                return minhaCDNLog;
            }
            return $"{minhaCDNLog}{Environment.NewLine}{Environment.NewLine}{agoraLog}";
        }

        public static async Task<string> ProcessOutputFormat(string outputFormat, string agoraFormat, EntityLog log)
        {
            switch (outputFormat.ToLower())
            {
                case "file":
                    log.FilePath = await FileUtils.SaveToFileAsync(agoraFormat);
                    return log.FilePath;
                case "response":
                    return log.AgoraLog;
                default:
                    throw new ArgumentException("Formato de saída inválido.", nameof(outputFormat));
            }
        }
    }
}
