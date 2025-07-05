using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingPlatform.Data.Models;

namespace RecipeSharingPlatform.Data.Configuration;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder
            .Property(r => r.IsDeleted)
            .HasDefaultValue(false);

        builder
            .HasQueryFilter(r => !r.IsDeleted);

        builder
            .HasOne(r => r.Category)
            .WithMany()
            .HasForeignKey(r => r.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(r => r.Author)
            .WithMany()
            .HasForeignKey(r => r.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(r => r.FavouritedByUsers)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UsersRecipes",
                j => j
                    .HasOne<IdentityUser>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.ClientCascade),
                j => j
                    .HasOne<Recipe>()
                    .WithMany()
                    .HasForeignKey("RecipeId")
                    .OnDelete(DeleteBehavior.ClientCascade),
                j =>
                {
                    j.HasKey("UserId", "RecipeId");
                    j.ToTable("UsersRecipes");
                });
    }
}