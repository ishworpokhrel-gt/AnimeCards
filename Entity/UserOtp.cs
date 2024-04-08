using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity
{
    public class UserOtpConfiguration : IEntityTypeConfiguration<UserOtp>
    {
        public void Configure(EntityTypeBuilder<UserOtp> builder)
        {
            builder.Property(a => a.Type)
                   .HasConversion(v => v.ToString(), v => (OtpType)Enum.Parse(typeof(OtpType), v));

            builder.HasOne(a => a.User)
                   .WithMany()
                   .HasForeignKey(a => a.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }

    [EntityTypeConfiguration(typeof(UserOtpConfiguration))]
    public class UserOtp : ApplicationBaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string OtpCode { get; set; }
        public string OtpModule { get; set; }
        public OtpType Type { get; set; }

    }

    public enum OtpType
    {
        SignUp,
        Login,
        ForgetPassword
    }
}
