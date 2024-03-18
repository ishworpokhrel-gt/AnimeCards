using System.ComponentModel.DataAnnotations;

namespace Common_Shared.CommonModel
{
    public class TokenModel
    {
        [Required]
        public string AccessToken { get; set; }
        public int AccessTokenExpiryInSeconds { get; set; }
        public string RefreshToken { get; set; }
        public int RefreshTokenExpiryInSeconds { get; set; }

    }
}
