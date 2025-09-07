using BookVerse.ViewModels.Genre;
using System.ComponentModel.DataAnnotations;
using static BookVerse.GCommon.ValidationConstants.Book;

namespace BookVerse.ViewModels.Book;

public class BookFormViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
    public string Description { get; set; } = null!;

    public string? CoverImageUrl { get; set; }

    [Required]
    public DateTime PublishedOn { get; set; }

    [Required]
    public int GenreId { get; set; }

    public ICollection<GenreViewModel>? Genres { get; set; } = null!;
}