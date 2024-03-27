using System.ComponentModel.DataAnnotations;

namespace Models.Account
{
    public class RegistrationRequestModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [Compare("ConfirmPassword", ErrorMessage = "Password and confirm password must be same.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [EmailAddress]
        public string Email { get; set; }
       
        public string PhoneNumber { get; set; }
    }
}
