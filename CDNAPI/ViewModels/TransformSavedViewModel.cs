using System;
using System.ComponentModel.DataAnnotations;

namespace CDNAPI.ViewModels
{
    public class TransformSavedViewModel
    {
        [Required(ErrorMessage = "O campo Input é obrigatório")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo OutputFormat é obrigatório")]
        [RegularExpression("^(file|response)$", ErrorMessage = "OutputFormat deve ser 'file' ou 'response'")]
        public string OutputFormat { get; set; }
    }
}