using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AccountDTOs
{
    public class AccountInfoDTO 
    {
        public string Phone{ get; set; }
    }
    public class AccountInfoDTOValidator : AbstractValidator<AccountInfoDTO>
    {
        public AccountInfoDTOValidator()
        {
            RuleFor(a => a.Phone).NotEmpty().NotNull();

        }
    }
}
