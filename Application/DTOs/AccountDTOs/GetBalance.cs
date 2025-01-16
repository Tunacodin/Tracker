using Application.DTOs.AuthDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AccountDTOs
{
    public class GetBalanceDTO 
    {
        public string AccountId { get; set; }
    }
    public class GetBalanceDTOValidator : AbstractValidator<GetBalanceDTO>
    {
        public GetBalanceDTOValidator()
        {
            RuleFor(a => a.AccountId).NotEmpty().NotNull();
          
        }
    }
}
