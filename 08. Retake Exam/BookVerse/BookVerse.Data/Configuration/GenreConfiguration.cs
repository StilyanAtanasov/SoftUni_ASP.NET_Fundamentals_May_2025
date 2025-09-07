using BookVerse.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookVerse.Data.Configuration;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder
            .HasData(
            new Genre { Id = 1, Name = "Fantasy" },
            new Genre { Id = 2, Name = "Thriller" },
            new Genre { Id = 3, Name = "Romance" },
            new Genre { Id = 4, Name = "Sci‑Fi" },
            new Genre { Id = 5, Name = "History" },
            new Genre { Id = 6, Name = "Non‑Fiction" }
        );
    }
}