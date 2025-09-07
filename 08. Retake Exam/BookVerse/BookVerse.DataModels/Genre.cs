using System.ComponentModel.DataAnnotations;
using static BookVerse.GCommon.ValidationConstants.Genre;

namespace BookVerse.DataModels;

public class Genre
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    public ICollection<Book> Books { get; set; } = new HashSet<Book>();
}