using Common_Shared.Attribute;
using System.ComponentModel.DataAnnotations;

namespace Animals.Account
{
    public class LogInRequestModel
    {
        [Required]
        [StringLength(32)]
        [UserName(ErrorMessage = "Username can only contain alphabets and numbers.")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
