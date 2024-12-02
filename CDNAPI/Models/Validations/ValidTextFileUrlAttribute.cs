using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CDNAPI.Models.Validations
{
    public class ValidTextFileUrlAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("A URL é obrigatória");

            var url = value.ToString();

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult))
                return new ValidationResult("URL inválida");

            if (!url.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                return new ValidationResult("A URL deve terminar com .txt");

            //var urlChecker = (IUrlChecker)validationContext.GetService(typeof(IUrlChecker));
            //if (!urlChecker.IsUrlAccessible(url).Result)
            //    return new ValidationResult("A URL fornecida não está acessível ou não existe");

            return ValidationResult.Success;
        }
    }
}
