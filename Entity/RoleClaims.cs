using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Entity
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaims>
    {
        public void Configure(EntityTypeBuilder<RoleClaims> builder)
        {
            builder.Property(e => e.Permissions)
                            .HasConversion(
                                v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                                v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()));
        }

    }
    [EntityTypeConfiguration(typeof(RoleClaimConfiguration))]
    public class RoleClaims : ApplicationBaseEntity
    {

        [ForeignKey(nameof(Role))]
        public string RoleId { get; set; }
        public ApplicationRole Role { get; set; }

        public List<string> Permissions { get; set; }

    }
}
