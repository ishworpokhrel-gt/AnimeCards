using System.ComponentModel.DataAnnotations;

namespace Models.Account
{
    public class ChangePasswordRequestModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [Compare("ConfirmNewPassword", ErrorMessage = "New password and confirm password must match.")]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}
