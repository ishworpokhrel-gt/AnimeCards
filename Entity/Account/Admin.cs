namespace Entity.Account
{
    public class Admin : ApplicationBaseEntity

    {
        public string RefreshToken { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string FullName { get; set; }
    }



}
