using CDNAPI.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace CDNAPI.ViewModels
{
    public class TransformLogViewModel
    {
        [Required(ErrorMessage = "O campo URL é obrigatório")]
        [ValidTextFileUrl]
        public string URL { get; set; }

        [Required(ErrorMessage = "O campo OutputFormat é obrigatório")]
        [RegularExpression("^(file|response)$", ErrorMessage = "OutputFormat deve ser 'file' ou 'response'")]
        public string OutputFormat { get; set; }
    }
}