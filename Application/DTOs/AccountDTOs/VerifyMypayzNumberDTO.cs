using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.DTOs.AccountDTOs
{
    public class VerifyMypayzNumberDTO
    {
        public string Phone { get; set; }
        public string ReceiverMypayzNumber { get; set; }
    }

    public class VerifyMypayzNumberDTOValidator : AbstractValidator<VerifyMypayzNumberDTO>
    {
        public VerifyMypayzNumberDTOValidator()
        {
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone number is required");

            RuleFor(x => x.ReceiverMypayzNumber)
                .NotEmpty()
                .WithMessage("Alıcı MyPayz numarası zorunludur");
        }
    }
}
