using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.DTOs.AccountDTOs
{
    public class AccountTransactionDTO
    {
        public string Phone { get; set; }
        public int Count { get; set; }
        public string? FirstDate { get; set; }
        public string? LastDate { get; set; }
    }

    public class AccountTransactionDTOValidator : AbstractValidator<AccountTransactionDTO>
    {
        private const string DateFormat = "yyyy-MM-dd";

        public AccountTransactionDTOValidator()
        {
            RuleFor(a => a.Phone).NotEmpty().NotNull();

            RuleFor(a => a.Count).GreaterThanOrEqualTo(0);

            When(
                a => !string.IsNullOrEmpty(a.FirstDate),
                () =>
                {
                    RuleFor(a => a.FirstDate)
                        .Must(date => IsValidDateFormat(date))
                        .WithMessage($"İlk Tarih '{DateFormat}' formatında olmalıdır.");
                }
            );

            When(
                a => !string.IsNullOrEmpty(a.LastDate),
                () =>
                {
                    RuleFor(a => a.LastDate)
                        .Must(date => IsValidDateFormat(date))
                        .WithMessage($"Son Tarih '{DateFormat}' formatında olmalıdır.");
                }
            );

            When(
                a => !string.IsNullOrEmpty(a.FirstDate) && !string.IsNullOrEmpty(a.LastDate),
                () =>
                {
                    RuleFor(a => DateTime.ParseExact(a.FirstDate!, DateFormat, null))
                        .LessThanOrEqualTo(a => DateTime.ParseExact(a.LastDate!, DateFormat, null))
                        .WithMessage("Son Tarih, İlk Tarih'ten büyük veya eşit olmalıdır.");
                }
            );
        }

        private bool IsValidDateFormat(string? date)
        {
            return DateTime.TryParseExact(
                date,
                DateFormat,
                null,
                System.Globalization.DateTimeStyles.None,
                out _
            );
        }
    }
}
