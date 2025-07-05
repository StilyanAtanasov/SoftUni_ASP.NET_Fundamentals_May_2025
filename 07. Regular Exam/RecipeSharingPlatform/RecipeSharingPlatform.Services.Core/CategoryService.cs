using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeSharingPlatform.Data;
using RecipeSharingPlatform.Services.Core.Contracts;
using RecipeSharingPlatform.ViewModels.Category;

namespace RecipeSharingPlatform.Services.Core;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context) => _context = context;

    public async Task<ICollection<CategoryViewModel>> GetAllategoriesReadOnlyAsync() =>
         await _context.Categories
            .AsNoTracking()
            .Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
            })
            .ToArrayAsync();
    
}