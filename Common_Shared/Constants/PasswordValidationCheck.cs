using System.Text.RegularExpressions;

namespace Common_Shared.Constants
{
    public class PasswordValidationCheck
    {
        public static bool PasswordValidators(string password)
        {
            if (password == null)
            {
                return false;
            }
            var isValid = Regex.IsMatch(password, @"[^(a-zA-Z0-9)]");
            if (isValid)
            {
                return true;
            }
            return false;
        }
    }
}
