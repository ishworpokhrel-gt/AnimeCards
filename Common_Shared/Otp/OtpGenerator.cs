using Entity;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace Common_Shared.Otp
{
    public class OtpGenerator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public OtpGenerator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Tuple<string> GenerateOtp()
        {
            var otpValue = new StringBuilder();
            var randomNumber = new byte[4];

            using (var otn = RandomNumberGenerator.Create())
            {
                otn.GetBytes(randomNumber);

                for (int i = 0; i < 4; i++)
                {
                    otpValue.Append(randomNumber[i] % 10);
                }

            }

            return Tuple.Create(otpValue.ToString());

        }
    }
}
