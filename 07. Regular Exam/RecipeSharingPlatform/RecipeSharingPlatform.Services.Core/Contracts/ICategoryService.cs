using RecipeSharingPlatform.ViewModels.Category;

namespace RecipeSharingPlatform.Services.Core.Contracts;

public interface ICategoryService
{
    Task<ICollection<CategoryViewModel>> GetAllategoriesReadOnlyAsync();
}