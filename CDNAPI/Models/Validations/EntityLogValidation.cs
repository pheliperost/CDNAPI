using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Models.Validations
{
    public class EntityLogValidation : AbstractValidator<EntityLog>
    {
        public EntityLogValidation()
        {
            RuleFor(f => f.URL)
               .NotEmpty().WithMessage("{PropertyName} é obrigatório.")
               .MaximumLength(2147483647).WithMessage("{PropertyName} excede o tamanho máximo permitido de 2GB.");

            RuleFor(f => f.MinhaCDNLog)
              .NotEmpty().WithMessage("{PropertyName} é obrigatório.")
              .MaximumLength(2147483647).WithMessage("{PropertyName} excede o tamanho máximo permitido de 2GB.");

            RuleFor(f => f.AgoraLog)
              .MaximumLength(2147483647).WithMessage("{PropertyName} excede o tamanho máximo permitido de 2GB.");

            RuleFor(f => f.FilePath)
             .MaximumLength(2147483647).WithMessage("{PropertyName} excede o tamanho máximo permitido de 2GB.");

        }
    }
}
