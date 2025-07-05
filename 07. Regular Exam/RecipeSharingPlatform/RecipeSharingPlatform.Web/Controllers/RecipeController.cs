using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeSharingPlatform.Services.Core.Contracts;
using RecipeSharingPlatform.Services.Core.Utils;
using RecipeSharingPlatform.ViewModels.Recipe;

namespace RecipeSharingPlatform.Web.Controllers;

public class RecipeController : BaseController
{
    private readonly IRecipeService _recipeService;
    private readonly ICategoryService _categoryService;

    public RecipeController(IRecipeService recipeService, ICategoryService categoryService)
    {
        _recipeService = recipeService;
        _categoryService = categoryService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        ICollection<RecipeCardViewModel> c = await _recipeService.GetAllRecipeCardsReadOnlyAsync(GetUserId());
        return View(c);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        RecipeDetailsViewModel? vm = await _recipeService.GetRecipeDetailsReadonlyAsync(id, GetUserId());
        if (vm == null) return NotFound();

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int id)
    {
        RecipeFormViewModel vm = new()
        {
            Categories = await _categoryService.GetAllategoriesReadOnlyAsync(),
            CreatedOn = DateTime.Now,
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RecipeFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = await _categoryService.GetAllategoriesReadOnlyAsync();
            return View(model);
        }

        string? userId = GetUserId();
        if (userId is null) return Forbid();

        ServiceResult r = await _recipeService.AddRecipeAsync(model, userId);
        if (r.Errors.Any())
        {
            foreach (var (field, errorMessage) in r.Errors) ModelState.AddModelError(field, errorMessage);
            model.Categories = await _categoryService.GetAllategoriesReadOnlyAsync();
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        string? userId = GetUserId();
        if (userId is null) return Forbid();

        ServiceResult<RecipeFormViewModel> sr = await _recipeService.GetRecipeForEditAsync(id, userId);
        if (!sr.Found) return NotFound();
        if (!sr.HasPermission) return Forbid();

        if (!sr.HasResult()) return StatusCode(500);

        RecipeFormViewModel vm = sr.Result!;
        vm.Categories = await _categoryService.GetAllategoriesReadOnlyAsync();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(RecipeFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = await _categoryService.GetAllategoriesReadOnlyAsync();
            return View(model);
        }

        string? userId = GetUserId();
        if (userId is null) return Forbid();

        ServiceResult r = await _recipeService.EditAsync(model, userId);

        if (!r.Found) return NotFound();
        if (!r.HasPermission) return Forbid();
        if (r.Errors.Any())
        {
            foreach (var (field, errorMessage) in r.Errors) ModelState.AddModelError(field, errorMessage);
            model.Categories = await _categoryService.GetAllategoriesReadOnlyAsync();
            return View(model);
        }

        return RedirectToAction(nameof(Details), new { model.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        string? userId = GetUserId();
        if (userId is null) return Forbid();

        ServiceResult<RecipeDeleteDetailsViewModel> sr = await _recipeService.GetRecipeDeleteDetailsReadonlyAsync(id, userId);
        if (!sr.Found) return NotFound();
        if (!sr.HasPermission) return Forbid();

        if (!sr.HasResult()) return StatusCode(500);
        return View(sr.Result);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        string? userId = GetUserId();
        if (userId is null) return Forbid();

        ServiceResult sr = await _recipeService.DeleteAsync(id, userId);
        if (!sr.Found) return NotFound();
        if (!sr.HasPermission) return Forbid();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Favourites()
    {
        string? userId = GetUserId();
        if (userId is null) return Forbid();

        ICollection<FavouriteRecipeCardViewModel> c = await _recipeService.GetAllFavouritesReadonlyAsync(userId);
        return View(c);
    }

    [HttpPost]
    public async Task<IActionResult> Save(int id)
    {
        string? userId = GetUserId();
        if (userId is null) return Forbid();

        ServiceResult r = await _recipeService.AddToFavouritesAsync(userId, id);
        if (!r.Found) return NotFound();
        if (!r.HasPermission) return Forbid();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int id)
    {
        string? userId = GetUserId();
        if (userId is null) return Forbid();

        ServiceResult r = await _recipeService.RemoveFromFavouritesAsync(userId, id);
        if (!r.Found) return NotFound();

        return RedirectToAction(nameof(Favourites));
    }
}