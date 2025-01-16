using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.DTOs.AccountDTOs
{
    public class SendMoneyToMypayzDTO
    {
        public string Phone { get; set; }
        public string ReceiverMypayzNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

    public class SendMoneyToMypayzDTOValidator : AbstractValidator<SendMoneyToMypayzDTO>
    {
        public SendMoneyToMypayzDTOValidator()
        {
            RuleFor(x => x.Phone).NotEmpty();

            RuleFor(x => x.ReceiverMypayzNumber)
                .NotEmpty()
                .WithMessage("Alıcı MyPayz numarası zorunludur");

            RuleFor(x => x.Amount)
                .NotEmpty()
                .WithMessage("Tutar zorunludur")
                .GreaterThan(0)
                .WithMessage("Tutar 0'dan büyük olmalıdır");
        }
    }
}
