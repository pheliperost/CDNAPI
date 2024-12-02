using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.ViewModels
{
    public class LogSearchViewModel
    {
        [Required(ErrorMessage = "O ID do log é obrigatório")]
        public Guid Id { get; set; }
    }
}
