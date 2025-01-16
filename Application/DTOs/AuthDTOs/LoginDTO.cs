
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AuthDTOs
{
    public class LoginDTO
    {
        public string Email { get; set; } 
        public string Password { get; set; }

    }
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(a => a.Email).NotEmpty().NotNull().MaximumLength(100);
            RuleFor(a => a.Password).NotEmpty().NotNull().MaximumLength(100);
        }
    }
}
