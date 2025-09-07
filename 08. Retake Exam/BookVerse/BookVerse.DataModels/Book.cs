using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BookVerse.GCommon.ValidationConstants.Book;

namespace BookVerse.DataModels;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(TitleMaxLength)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; } = null!;

    public string? CoverImageUrl { get; set; }

    [Required]
    [ForeignKey(nameof(Publisher))]
    public string PublisherId { get; set; } = null!;

    [Required]
    public IdentityUser Publisher { get; set; } = null!;

    [Required]
    public DateTime PublishedOn { get; set; }

    [Required]
    [ForeignKey(nameof(Genre))]
    public int GenreId { get; set; }

    [Required]
    public Genre Genre { get; set; } = null!;

    [Required]
    public bool IsDeleted { get; set; }

    public ICollection<IdentityUser> LikedByUsers { get; set; } = new HashSet<IdentityUser>();
}