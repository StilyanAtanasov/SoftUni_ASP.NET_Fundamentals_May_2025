namespace RecipeSharingPlatform.ViewModels.Recipe;

public class FavouriteRecipeCardViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string Category { get; set; } = null!;
}