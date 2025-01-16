using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.DTOs.AccountDTOs
{
    public class RequestMoneyQrDTO
    {
        public string Phone { get; set; }
        public int Amount { get; set; }
    }

    public class RequestMoneyQrDTOValidator : AbstractValidator<RequestMoneyQrDTO>
    {
        public RequestMoneyQrDTOValidator()
        {
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Telefon numarası zorunludur.");

            RuleFor(x => x.Amount)
                .NotEmpty()
                .WithMessage("Tutar zorunludur.")
                .GreaterThan(0)
                .WithMessage("Tutar 0'dan büyük olmalıdır.");
        }
    }
}
