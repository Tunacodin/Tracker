using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.DTOs.AuthDTOs
{
    public class CodeDTO : AccountLoginDTO
    {
        public string Otp { get; set; }
        public string? CustomerId { get; set; }
    }

    public class CodeDTOValidator : AbstractValidator<CodeDTO>
    {
        public CodeDTOValidator()
        {
            RuleFor(a => a.Phone).NotEmpty().NotNull().MaximumLength(100);
            RuleFor(a => a.Password).NotEmpty().NotNull().MaximumLength(100);
            RuleFor(a => a.Otp).NotEmpty().NotNull().MaximumLength(100);
        }
    }
}
