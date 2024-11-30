using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Model
{
    public class MinhaCDNLog
    {
        [Key]
        public Guid Id { get; set; }
        public string clientId { get; set; }
        public string provider { get; set; }
        public string httpmethod { get; set; }
        public string statuscode { get; set; }
        public string uripath { get; set; }
        public string cachestatus { get; set; }
        public string timetaken { get; set; }
    }
}
