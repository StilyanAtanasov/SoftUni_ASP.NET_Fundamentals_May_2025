using RecipeSharingPlatform.ViewModels.Category;
using System.ComponentModel.DataAnnotations;
using static RecipeSharingPlatform.GCommon.ValidationConstants.Recipe;

namespace RecipeSharingPlatform.ViewModels.Recipe;

public class RecipeFormViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
    public string Title { get; set; } = null!;

    public string? ImageUrl { get; set; }

    [Required]
    [StringLength(InstructionsMaxLength, MinimumLength = InstructionsMinLength)]
    public string Instructions { get; set; } = null!;

    [Required]
    public DateTime CreatedOn { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public ICollection<CategoryViewModel>? Categories { get; set; }
}