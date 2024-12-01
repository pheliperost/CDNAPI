using System;

namespace CDNAPI.Models
{
    public class AgoraLog : Entity
    {
        public Guid MinhaCDNLogId { get; set; }
        public string clientId { get; set; }
        public string provider { get; set; }
        public string httpmethod { get; set; }
        public string statuscode { get; set; }
        public string uripath { get; set; }
        public string cachestatus { get; set; }
        public string timetaken { get; set; }


        //EF Relations 
        public MinhaCDNLog MinhaCDNLog { get; set; }
    }
}
