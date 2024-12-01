using CDNAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.LogConverter
{
    public static class Converter
    {
        //public List<String> ConvertFromMinhaCDNtoAgora()
        //{

        //}
        public static MinhaCDNLog ConvertStringtoModel(string logLine)
        {
            string[] parts = logLine.Split('|');

            string clientId = parts[0];
            string statuscode = parts[1];
            string cachestatus = parts[2];
            string requestLine = parts[3].Trim('"');
            //double timetaken = double.Parse(parts[4], System.Globalization.CultureInfo.InvariantCulture);
            string timetaken = parts[4];

            string[] requestParts = requestLine.Split(' ', 2);
            string httpmethod = requestParts[0];
            string uripath = requestParts[1].Split(' ')[0];

            return new MinhaCDNLog
            {
                clientId = clientId,
                statuscode = statuscode,
                cachestatus = cachestatus,
                httpmethod = httpmethod,
                uripath = uripath,
                timetaken = timetaken
            };
        }
    }
}
