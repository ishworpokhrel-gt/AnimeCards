using System.ComponentModel.DataAnnotations;

namespace Common_Shared.Attribute
{
    public class DigitonlyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is string str)
            {

                if (IsNumeric(str))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return new ValidationResult("The value must be a string.");
        }

        private bool IsNumeric(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

