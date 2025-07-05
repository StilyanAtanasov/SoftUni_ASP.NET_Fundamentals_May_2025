namespace RecipeSharingPlatform.ViewModels.Recipe;

public class RecipeDeleteDetailsViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string AuthorId { get; set; } = null!;
}