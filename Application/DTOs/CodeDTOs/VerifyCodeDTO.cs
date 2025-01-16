using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.DTOs.CodeDTOs
{
    public class VerifyCodeDTO
    {
        public string CodeId { get; set; }
        public string? CustomerEmail { get; set; }
    }

    public class VerifyCodeDTOValidator : AbstractValidator<VerifyCodeDTO>
    {
        public VerifyCodeDTOValidator()
        {
            RuleFor(x => x.CodeId).NotEmpty().WithMessage("Code ID zorunludur");

            RuleFor(x => x.CustomerEmail)
                .NotEmpty()
                .WithMessage("Müşteri e-postası zorunludur")
                .EmailAddress()
                .WithMessage("Geçersiz e-posta formatı");
        }
    }
}
