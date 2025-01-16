using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utilities.Constants;
using Domain.HelperClass;
using FluentValidation;

namespace Application.DTOs.AuthDTOs
{
    public class AccountLoginDTO
    {
        public string Phone { get; set; }
        public string Password { get; set; }
        public string? AccountName { get; set; }
        public MobileDevice? MobileDevice { get; set; }
    }

    public class AccountLoginDTOValidator : AbstractValidator<AccountLoginDTO>
    {
        public AccountLoginDTOValidator()
        {
            RuleFor(a => a.Phone)
                .NotEmpty()
                .NotNull()
                .MaximumLength(100)
                .Must(phone => !phone.StartsWith("0") && !phone.StartsWith("+90"))
                .WithMessage("Telefon numarası 0 ile başlayamaz");
            RuleFor(a => a.Password).NotEmpty().NotNull().MaximumLength(100);
        }
    }

    public class CreateAccountDTO : AccountLoginDTO
    {
        public string? CustomerId { get; set; }
    }

    public class AddAccounToCustomerDTO : CreateAccountDTO { }

    public class RemoveAccountFromCustomerDTO
    {
        public string Phone { get; set; }
        public string CustomerId { get; set; }
    }

    public class RemoveAccountFromCustomerDTOValidator
        : AbstractValidator<RemoveAccountFromCustomerDTO>
    {
        public RemoveAccountFromCustomerDTOValidator()
        {
            RuleFor(a => a.Phone).NotEmpty().NotNull().MaximumLength(100);
            RuleFor(a => a.CustomerId).NotEmpty().NotNull().MaximumLength(100);
        }
    }

    public class AddAccounToCustomerDTOValidator : AbstractValidator<AddAccounToCustomerDTO>
    {
        public AddAccounToCustomerDTOValidator()
        {
            RuleFor(a => a.Phone)
                .NotEmpty()
                .NotNull()
                .MaximumLength(100)
                .Must(phone => !phone.StartsWith("0") && !phone.StartsWith("+90"))
                .WithMessage("Telefon numarası 0 ile başlayamaz");
            RuleFor(a => a.Password).NotEmpty().NotNull().MaximumLength(100);
        }
    }

    public class DeleteAccountDTO
    {
        public string Phone { get; set; }
    }

    public class DeleteAccountDTOValidator : AbstractValidator<DeleteAccountDTO>
    {
        public DeleteAccountDTOValidator()
        {
            RuleFor(a => a.Phone).NotEmpty().NotNull().MaximumLength(100);
        }
    }
}
