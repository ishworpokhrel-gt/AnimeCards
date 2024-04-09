using System.ComponentModel.DataAnnotations;

namespace Models.Admin
{
    public class UpdateCustomerRequestModel
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        [MaxLength(15,ErrorMessage ="Cannot exceed more than 15 characters")]
        public string PhoneNumber { get; set; }
        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
