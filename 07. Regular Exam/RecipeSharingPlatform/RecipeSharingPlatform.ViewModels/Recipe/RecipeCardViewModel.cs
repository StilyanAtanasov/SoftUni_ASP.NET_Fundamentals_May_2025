namespace RecipeSharingPlatform.ViewModels.Recipe;

public class RecipeCardViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public bool IsAuthor { get; set; }

    public bool IsSaved { get; set; }

    public int SavedCount { get; set; }

    public string Category { get; set; } = null!;
}