using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.DTOs.CustomerDTOs
{
    public class CreateCustomerDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string? WhoAdded { get; set; }
        public bool IsMaster { get; set; } = false;
    }

    public class CreateCustomerDTOValidator : AbstractValidator<CreateCustomerDTO>
    {
        public CreateCustomerDTOValidator()
        {
            RuleFor(a => a.FullName).NotEmpty().NotNull().MaximumLength(100);
            RuleFor(a => a.Password).Equal(b => b.PasswordConfirm).NotNull().NotEmpty();
            RuleFor(a => a.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(a => a.UserName).NotEmpty().NotNull();
        }
    }
}
