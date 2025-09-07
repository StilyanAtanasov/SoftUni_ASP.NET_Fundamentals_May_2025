using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static BookVerse.GCommon.SeedingConstants;

namespace BookVerse.Data.Configuration;

public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
{
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        builder
            .HasData(new IdentityUser
            {
                Id = DefaultUserId,
                UserName = "admin@bookverse.com",
                NormalizedUserName = "ADMIN@BOOKVERSE.COM",
                Email = "admin@bookverse.com",
                NormalizedEmail = "ADMIN@BOOKVERSE.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>()
                    .HashPassword(new IdentityUser { UserName = "admin@bookverse.com" }, "Admin123!")
            });
    }
}