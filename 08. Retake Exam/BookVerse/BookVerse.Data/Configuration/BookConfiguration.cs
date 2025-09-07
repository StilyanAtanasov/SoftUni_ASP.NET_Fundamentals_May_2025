using BookVerse.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static BookVerse.GCommon.SeedingConstants;
using static BookVerse.GCommon.ValidationConstants.Book;

namespace BookVerse.Data.Configuration;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder
            .Property(b => b.IsDeleted)
            .HasDefaultValue(IsDeletedDefaultValue);

        builder
            .HasMany(b => b.LikedByUsers)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UsersBooks",
                j => j
                    .HasOne<IdentityUser>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.ClientCascade),
                j => j
                    .HasOne<Book>()
                    .WithMany()
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.ClientCascade),
                j =>
                {
                    j.HasKey("UserId", "BookId");
                    j.ToTable("UsersBooks");
                });

        builder
            .HasQueryFilter(b => !b.IsDeleted);

        builder.HasData(
            new Book
            {
                Id = 1,
                Title = "Whispers of the Mountain",
                Description = "Emily Harper (released 2015): A quiet village, a hidden path, and a choice that changes everything.",
                CoverImageUrl = "https://m.media-amazon.com/images/I/9187Qn8bL6L._UF1000,1000_QL80_.jpg",
                PublisherId = DefaultUserId,
                PublishedOn = DateTime.Now,
                GenreId = 1,
                IsDeleted = false
            },
            new Book
            {
                Id = 2,
                Title = "Shadows in the Fog",
                Description = "Michael Turner (released: 2017): An investigator follows a trail of secrets through a city shrouded in mystery.",
                CoverImageUrl = "https://m.media-amazon.com/images/I/719g0mh9f2L._UF1000,1000_QL80_.jpg",
                PublisherId = DefaultUserId,
                PublishedOn = DateTime.Now,
                GenreId = 2,
                IsDeleted = false
            },
            new Book
            {
                Id = 3,
                Title = "Letters from the Heart",
                Description = "Sarah Collins (released 2020): A touching story about love, distance, and the power of written words.",
                CoverImageUrl = "https://m.media-amazon.com/images/I/71zwodP9GzL._UF1000,1000_QL80_.jpg",
                PublisherId = DefaultUserId,
                PublishedOn = DateTime.Now,
                GenreId = 3,
                IsDeleted = false
            }
        );
    }
}