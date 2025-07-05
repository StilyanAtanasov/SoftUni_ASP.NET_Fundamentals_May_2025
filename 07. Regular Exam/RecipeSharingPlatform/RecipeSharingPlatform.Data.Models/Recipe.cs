using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RecipeSharingPlatform.GCommon.ValidationConstants.Recipe;

namespace RecipeSharingPlatform.Data.Models;

public class Recipe
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(TitleMaxLength)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(InstructionsMaxLength)]
    public string Instructions { get; set; } = null!;

    public string? ImageUrl { get; set; }

    [Required]
    [ForeignKey(nameof(Author))]
    public string AuthorId { get; set; } = null!;

    [Required]
    public IdentityUser Author { get; set; } = null!;

    [Required]
    public DateTime CreatedOn { get; set; }

    [Required]
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    [Required]
    public Category Category { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public ICollection<IdentityUser> FavouritedByUsers { get; set; } = new HashSet<IdentityUser>();
}