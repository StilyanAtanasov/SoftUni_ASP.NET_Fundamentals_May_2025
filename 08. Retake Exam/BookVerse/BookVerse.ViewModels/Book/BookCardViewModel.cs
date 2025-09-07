using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BookVerse.GCommon.ValidationConstants;

namespace BookVerse.ViewModels.Book;

public class BookCardViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? CoverImageUrl { get; set; }

    public int SavedCount { get; set; }

    public bool IsAuthor { get; set; }

    public bool IsSaved { get; set; }

    public string Genre { get; set; } = null!;
}