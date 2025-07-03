using Horizons.Services.Core.Contracts;
using Horizons.Services.Core.Utils;
using Horizons.Web.ViewModels.Destination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizons.Web.Controllers;

public class DestinationController : BaseController
{
    private readonly IDestinationService _destinationService;
    private readonly ITerrainService _terrainService;

    public DestinationController(IDestinationService destinationService, ITerrainService terrainService)
    {
        _destinationService = destinationService;
        _terrainService = terrainService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string? userId = GetUserId();

        ICollection<DestinationCardViewModel> destinationCard = await _destinationService.GetDestinationCardsReadOnlyAsync(userId);
        return View(destinationCard);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        return View(new DestinationFormViewModel
        {
            Terrains = await _terrainService.GetAllTerrainTypesReadOnlyAsync()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Add(DestinationFormViewModel model)
    {
        string? userId = GetUserId();
        if (!ModelState.IsValid || userId is null)
        {
            model.Terrains = await _terrainService.GetAllTerrainTypesReadOnlyAsync();
            return View(model);
        }

        await _destinationService.AddDestination(model, userId);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        string? userId = GetUserId();

        DestinationDetailsViewModel? vm = await _destinationService.GetDestinationDetailsReadonlyAsync(id, userId);
        if (vm == null) return NotFound();

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        DestinationFormViewModel? vm = await _destinationService.GetDestinationAsync(id);
        if (vm == null) return NotFound();

        vm.Terrains = await _terrainService.GetAllTerrainTypesReadOnlyAsync();

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(DestinationFormViewModel model)
    {
        string? userId = GetUserId();
        if (!ModelState.IsValid || userId is null)
        {
            model.Terrains = await _terrainService.GetAllTerrainTypesReadOnlyAsync();
            return View(model);
        }

        ServiceResult r = await _destinationService.EditDestinationAsync(model, userId);
        if (!r.HasPermission) return Forbid();
        if (!r.Found) return NotFound();
        if (r.Errors.Any())
        {
            foreach (var (name, massage) in r.Errors) ModelState.AddModelError(name, massage);
            model.Terrains = await _terrainService.GetAllTerrainTypesReadOnlyAsync();
            return View(model);
        }

        return RedirectToAction(nameof(Details), new { model.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        string? userId = GetUserId();
        if (userId is null) return Forbid();

        DestinationDeleteDetailsViewModel? vm = await _destinationService.GetDestinationDeleteDetailsReadonlyAsync(id);
        if (vm is null) return NotFound();
        if (vm.PublisherId != userId) return Forbid();

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DestinationDeleteDetailsViewModel model)
    {
        string? userId = GetUserId();
        if (userId is null) return Forbid();

        ServiceResult r = await _destinationService.DeleteDestinationAsync(model.Id, userId);
        if (!r.HasPermission) return Forbid();
        if (!r.Found) return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Favorites()
    {
        string? userId = GetUserId();
        if (userId is null) return Forbid();

        ICollection<DestinationFavoriteCardViewModel> c = await _destinationService.GetFavoriteDestinationsReadonlyAsync(userId);
        return View(c);
    }

    [HttpPost]
    public async Task<IActionResult> AddToFavorites(int destinationId)
    {
        string? userId = GetUserId();
        if (userId is null) return Forbid();

        ServiceResult r = await _destinationService.AddFavoriteDestinationAsync(destinationId, userId);
        if (!r.Found) return NotFound();
        if (!r.HasPermission) return Forbid();

        string referer = Request.Headers["Referer"].ToString();
        return string.IsNullOrEmpty(referer) ? RedirectToAction(nameof(Index), "Home") : Redirect(referer);
    }


    [HttpPost]
    public async Task<IActionResult> RemoveFromFavorites(int id)
    {
        string? userId = GetUserId();
        if (userId is null) return Forbid();

        ServiceResult r = await _destinationService.RemoveFavoriteDestinationAsync(id, userId);
        if (!r.Found) return NotFound();

        return RedirectToAction(nameof(Favorites));
    }
}
