using System.ComponentModel.DataAnnotations;

namespace Models.Admin
{
    public class ChangeCustomerPasswordRequestModel
    {
        [Required]
        [Compare(nameof(ConfirmPassword))]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string AdminPassword { get; set; }
    }
}
