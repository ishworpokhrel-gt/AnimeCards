namespace Entity
{
    public class PreviousPassword : ApplicationBaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string HashPassword { get; set; }
    }
}
