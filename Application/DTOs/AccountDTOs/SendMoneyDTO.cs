using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.DTOs.AccountDTOs
{
    public class SendMoneyDTO
    {
        public string Phone { get; set; }
        public string IBAN { get; set; }
        public decimal Amount { get; set; }
        public string NameSurname { get; set; }
        public string? Description { get; set; }
    }

    public class SendMoneyDTOValidator : AbstractValidator<SendMoneyDTO>
    {
        public SendMoneyDTOValidator()
        {
            RuleFor(x => x.Phone).NotEmpty();

            RuleFor(x => x.IBAN)
                .NotEmpty()
                .WithMessage("IBAN zorunludur")
                .MinimumLength(26)
                .WithMessage("IBAN en az 26 karakter olmalıdır");

            RuleFor(x => x.Amount)
                .NotEmpty()
                .WithMessage("Tutar zorunludur")
                .GreaterThan(0)
                .WithMessage("Tutar 0'dan büyük olmalıdır");

            RuleFor(x => x.NameSurname).NotEmpty();
        }
    }
}
