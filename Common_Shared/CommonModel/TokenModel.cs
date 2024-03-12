using System.ComponentModel.DataAnnotations;

namespace Common_Shared.CommonModel
{
    public class TokenModel
    {
        [Required]
        public string AccessToken { get; set; }
        public int AccessTokenExpiryInSeconds { get; set; }

    }
}
