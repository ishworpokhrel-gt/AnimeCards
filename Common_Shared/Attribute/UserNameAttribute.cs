using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Common_Shared.Attribute
{
    public class UserNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is string obj)
            {
                if (string.IsNullOrWhiteSpace(obj))
                {
                    return new ValidationResult(ErrorMessage);
                }

                if (!Regex.IsMatch(obj, @"^[a-zA-Z0-9]+$"))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;


        }
    }
}
