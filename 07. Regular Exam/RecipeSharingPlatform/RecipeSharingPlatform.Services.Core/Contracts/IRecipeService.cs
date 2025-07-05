using RecipeSharingPlatform.Services.Core.Utils;
using RecipeSharingPlatform.ViewModels.Recipe;

namespace RecipeSharingPlatform.Services.Core.Contracts;

public interface IRecipeService
{
    Task<ICollection<RecipeCardViewModel>> GetAllRecipeCardsReadOnlyAsync(string? userId);

    Task<RecipeDetailsViewModel?> GetRecipeDetailsReadonlyAsync(int recipeId, string? userId);

    Task<ServiceResult<RecipeDeleteDetailsViewModel>> GetRecipeDeleteDetailsReadonlyAsync(int recipeId, string userId);

    Task<ServiceResult<RecipeFormViewModel>> GetRecipeForEditAsync(int recipeId, string userId);

    Task<ICollection<FavouriteRecipeCardViewModel>> GetAllFavouritesReadonlyAsync(string userId);

    Task<ServiceResult> AddToFavouritesAsync(string userId, int recipeId);

    Task<ServiceResult> RemoveFromFavouritesAsync(string userId, int recipeId);

    Task<ServiceResult> AddRecipeAsync(RecipeFormViewModel model, string userId);

    Task<ServiceResult> EditAsync(RecipeFormViewModel model, string userId);

    Task<ServiceResult> DeleteAsync(int recipeId, string userId);
}