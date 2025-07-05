using System.ComponentModel.DataAnnotations;
using static RecipeSharingPlatform.GCommon.ValidationConstants.Category;

namespace RecipeSharingPlatform.Data.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    public ICollection<Recipe> Recipes { get; set; } = new HashSet<Recipe>();
}