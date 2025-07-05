using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeSharingPlatform.Data;
using RecipeSharingPlatform.Data.Models;
using RecipeSharingPlatform.Services.Core.Contracts;
using RecipeSharingPlatform.Services.Core.Utils;
using RecipeSharingPlatform.ViewModels.Recipe;

namespace RecipeSharingPlatform.Services.Core;

public class RecipeService : IRecipeService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public RecipeService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<ServiceResult> AddToFavouritesAsync(string userId, int recipeId)
    {
        Recipe? r = await _context.Recipes
            .Include(r => r.FavouritedByUsers)
            .FirstOrDefaultAsync(r => r.Id == recipeId);

        if (r is null) return ServiceResult.NotFound();
        if (r.AuthorId == userId) return ServiceResult.Forbidden();

        if (r.FavouritedByUsers.All(u => u.Id != userId))
        {
            r.FavouritedByUsers.Add((await _userManager.FindByIdAsync(userId))!);
            await _context.SaveChangesAsync();
        }

        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> RemoveFromFavouritesAsync(string userId, int recipeId)
    {
        Recipe? r = await _context.Recipes
            .Include(r => r.FavouritedByUsers)
            .FirstOrDefaultAsync(r => r.Id == recipeId);

        if (r is null) return ServiceResult.NotFound();

        r.FavouritedByUsers.Remove((await _userManager.FindByIdAsync(userId))!);
        await _context.SaveChangesAsync();

        return ServiceResult.Ok();
    }

    public async Task<ServiceResult<RecipeDeleteDetailsViewModel>> GetRecipeDeleteDetailsReadonlyAsync(int recipeId, string userId)
    {
        Recipe? r = await _context.Recipes
            .Include(r => r.Author)
            .FirstOrDefaultAsync(r => r.Id == recipeId);

        if (r == null) return ServiceResult<RecipeDeleteDetailsViewModel>.NotFound();
        if (r.AuthorId != userId) return ServiceResult<RecipeDeleteDetailsViewModel>.Forbidden();

        RecipeDeleteDetailsViewModel vm = new()
        {
            Id = r.Id,
            Title = r.Title,
            Author = r.Author.Email!,
            AuthorId = r.AuthorId,
        };

        return ServiceResult<RecipeDeleteDetailsViewModel>.Ok(vm);
    }

    public async Task<ServiceResult<RecipeFormViewModel>> GetRecipeForEditAsync(int recipeId, string userId)
    {
        Recipe? r = await _context.Recipes
            .AsNoTracking()
            .Where(r => r.Id == recipeId)
            .FirstOrDefaultAsync();

        if (r is null) return ServiceResult<RecipeFormViewModel>.NotFound();
        if (r.AuthorId != userId) return ServiceResult<RecipeFormViewModel>.Forbidden();

        RecipeFormViewModel vm = new()
        {
            Id = r.Id,
            Title = r.Title,
            CategoryId = r.CategoryId,
            ImageUrl = r.ImageUrl,
            Instructions = r.Instructions,
            CreatedOn = r.CreatedOn
        };

        return ServiceResult<RecipeFormViewModel>.Ok(vm);
    }

    public async Task<ICollection<FavouriteRecipeCardViewModel>> GetAllFavouritesReadonlyAsync(string userId) =>
         await _context.Recipes
            .AsNoTracking()
            .Include(r => r.Category)
            .Where(r => r.FavouritedByUsers.Any(u => u.Id == userId))
            .Select(r => new FavouriteRecipeCardViewModel
            {
                Id = r.Id,
                Title = r.Title,
                Category = r.Category.Name,
                ImageUrl = r.ImageUrl,
            })
            .ToArrayAsync();

    public async Task<ICollection<RecipeCardViewModel>> GetAllRecipeCardsReadOnlyAsync(string? userId) =>
         await _context.Recipes
            .AsNoTracking()
            .Include(r => r.Category)
            .Include(r => r.FavouritedByUsers)
            .Select(r => new RecipeCardViewModel
            {
                Id = r.Id,
                Title = r.Title,
                Category = r.Category.Name,
                ImageUrl = r.ImageUrl,
                IsAuthor = userId != null && r.AuthorId == userId,
                IsSaved = r.FavouritedByUsers.Any(u => u.Id == userId),
                SavedCount = r.FavouritedByUsers.Count
            })
            .ToArrayAsync();

    public async Task<RecipeDetailsViewModel?> GetRecipeDetailsReadonlyAsync(int recipeId, string? userId)
    {
        Recipe? r = await _context.Recipes
            .Include(r => r.Author)
            .Include(r => r.Category)
            .Include(r => r.FavouritedByUsers)
            .FirstOrDefaultAsync(r => r.Id == recipeId);

        if (r == null) return null;

        RecipeDetailsViewModel vm = new()
        {
            Id = r.Id,
            Title = r.Title,
            Category = r.Category.Name,
            Instructions = r.Instructions,
            ImageUrl = r.ImageUrl,
            Author = r.Author.Email!,
            CreatedOn = r.CreatedOn,
            IsAuthor = userId != null && r.AuthorId == userId,
            IsSaved = userId != null && r.FavouritedByUsers.Any(u => u.Id == userId)
        };

        return vm;
    }
    public async Task<ServiceResult> AddRecipeAsync(RecipeFormViewModel model, string userId)
    {
        if (DateTime.Compare(DateTime.Now, model.CreatedOn) < 0)
            return ServiceResult.Failure(new() { [nameof(model.CreatedOn)] = "Date cannot be set to future!" });

        Recipe r = new()
        {
            Id = model.Id,
            Title = model.Title,
            CategoryId = model.CategoryId,
            Instructions = model.Instructions,
            ImageUrl = model.ImageUrl,
            AuthorId = userId,
            CreatedOn = model.CreatedOn
        };

        await _context.Recipes.AddAsync(r);
        await _context.SaveChangesAsync();

        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> EditAsync(RecipeFormViewModel model, string userId)
    {
        if (DateTime.Compare(DateTime.Now, model.CreatedOn) < 0)
            return ServiceResult.Failure(new() { [nameof(model.CreatedOn)] = "Date cannot be set to future!" });

        Recipe? r = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == model.Id);
        if (r is null) return ServiceResult.NotFound();
        if (r.AuthorId != userId) return ServiceResult.Forbidden();

        r.Title = model.Title;
        r.CategoryId = model.CategoryId;
        r.Instructions = model.Instructions;
        r.ImageUrl = model.ImageUrl;
        r.CreatedOn = model.CreatedOn;

        await _context.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DeleteAsync(int recipeId, string userId)
    {
        Recipe? r = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);

        if (r is null) return ServiceResult.NotFound();
        if (r.AuthorId != userId) return ServiceResult.Forbidden();

        r.IsDeleted = true;
        await _context.SaveChangesAsync();

        return ServiceResult.Ok();
    }
}