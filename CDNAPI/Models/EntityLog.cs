using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Models
{
    public class EntityLog : Entity
    {
        public string MinhaCDNLog { get; set; }
        public string AgoraLog { get; set; }
        public DateTime CreatedAt { get; set; }
        public string URL { get; set; }
        public string FilePath { get; set; }

    }
}
