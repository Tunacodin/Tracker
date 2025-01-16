using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities.Helper
{
    public static class ExtensionClass
    {
        public static List<string> GetValidationErrors(this List<ValidationFailure> validationResults)
        {
            List<string> ValidationMessages = new List<string>();
            foreach (ValidationFailure failure in validationResults)
            {
                ValidationMessages.Add(failure.ErrorMessage);
            }
            return ValidationMessages;
        }
    }
}
