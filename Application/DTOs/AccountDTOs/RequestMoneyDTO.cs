using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.DTOs.AccountDTOs
{
    public class RequestMoneyDTO
    {
        public string Phone { get; set; }
        public string FromWho { get; set; }
        public decimal Amount { get; set; }
        public string? Comment { get; set; }
    }

    public class RequestMoneyDTOValidator : AbstractValidator<RequestMoneyDTO>
    {
        public RequestMoneyDTOValidator()
        {
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Telefon numarası zorunludur.");

            RuleFor(x => x.FromWho).NotEmpty().WithMessage("Alıcı MyPayz numarası zorunludur.");

            RuleFor(x => x.Amount)
                .NotEmpty()
                .WithMessage("Tutar zorunludur.")
                .GreaterThan(0)
                .WithMessage("Tutar 0'dan büyük olmalıdır.");

            RuleFor(x => x.Comment).MaximumLength(100).WithMessage("Yorum 100 karakteri geçemez.");
        }
    }
}
