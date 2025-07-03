using Horizons.Data;
using Horizons.Data.Models;
using Horizons.Services.Core.Contracts;
using Horizons.Services.Core.Utils;
using Horizons.Web.ViewModels.Destination;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static Horizons.GCommon.ValidationConstants.Destination;

namespace Horizons.Services.Core;

public class DestinationService : IDestinationService
{
    private readonly ApplicationDbContext _context;

    public DestinationService(ApplicationDbContext context) => _context = context;

    public async Task AddDestination(DestinationFormViewModel model, string userId)
    {
        Destination destination = new()
        {
            Name = model.Name,
            ImageUrl = model.ImageUrl,
            Description = model.Description,
            TerrainId = model.TerrainId,
            PublishedOn = DateTime.ParseExact(model.PublishedOn, DateTimeFormat, CultureInfo.InvariantCulture),
            PublisherId = userId
        };

        await _context.Destinations.AddAsync(destination);
        await _context.SaveChangesAsync();
    }

    public async Task<DestinationDetailsViewModel?> GetDestinationDetailsReadonlyAsync(int id, string? userId)
    {

        Destination? d = await _context.Destinations
            .AsNoTracking()
            .Include(d => d.Terrain)
            .Include(d => d.Publisher)
            .FirstOrDefaultAsync(d => d.Id == id);

        ICollection<int>? favorites = userId is null ? null : await _context.UsersDestinations
            .Where(ud => ud.UserId == userId)
            .Select(ud => ud.DestinationId)
            .ToArrayAsync();

        if (d is null) return null;

        DestinationDetailsViewModel vm = new()
        {
            Id = d.Id,
            Name = d.Name,
            ImageUrl = d.ImageUrl,
            Terrain = d.Terrain.Name,
            PublishedOn = d.PublishedOn,
            Description = d.Description,
            Publisher = d.Publisher.Email!,
            IsPublisher = d.PublisherId == userId,
            IsFavorite = favorites != null && favorites.Contains(d.Id)
        };

        return vm;
    }

    public async Task<ServiceResult> DeleteDestinationAsync(int id, string userId)
    {
        Destination? d = await _context.Destinations.FirstOrDefaultAsync(d => d.Id == id);

        if (d is null) return new ServiceResult() { Found = false };
        if (d.PublisherId != userId) return new ServiceResult() { HasPermission = false };

        d.IsDeleted = true;
        await _context.SaveChangesAsync();
        return new ServiceResult();
    }

    public async Task<ServiceResult> EditDestinationAsync(DestinationFormViewModel model, string userId)
    {
        Destination? d = await _context.Destinations.FirstOrDefaultAsync(d => d.Id == model.Id);
        bool success = DateTime.TryParse(model.PublishedOn, out DateTime date);

        if (d is null) return new ServiceResult() { Found = false };
        if (d.PublisherId != userId) return new ServiceResult() { HasPermission = false };
        if (!success || DateTime.Compare(DateTime.Now, date) < 0)
            return new ServiceResult { Errors = { [nameof(model.PublishedOn)] = "Invalid date!" } };

        d.Description = model.Description;
        d.ImageUrl = model.ImageUrl;
        d.Name = model.Name;
        d.PublishedOn = date;
        d.TerrainId = model.TerrainId;

        await _context.SaveChangesAsync();
        return new ServiceResult();
    }

    public async Task<DestinationFormViewModel?> GetDestinationAsync(int id)
    {
        Destination? d = await _context.Destinations.FirstOrDefaultAsync(d => d.Id == id);
        if (d is null) return null;

        DestinationFormViewModel vm = new()
        {
            Id = d.Id,
            Name = d.Name,
            ImageUrl = d.ImageUrl,
            Description = d.Description,
            TerrainId = d.TerrainId,
            PublishedOn = d.PublishedOn.ToString(DateTimeFormat),
            PublisherId = d.PublisherId
        };

        return vm;
    }

    public async Task<ICollection<DestinationCardViewModel>> GetDestinationCardsReadOnlyAsync(string? userId)
    {
        ICollection<int>? favorites = userId is null ? null : await _context.UsersDestinations
            .Where(ud => ud.UserId == userId)
            .Select(ud => ud.DestinationId)
            .ToArrayAsync();

        return await _context.Destinations
           .AsNoTracking()
           .Include(d => d.Terrain)
           .Select(d => new DestinationCardViewModel()
           {
               Id = d.Id,
               Name = d.Name,
               ImageUrl = d.ImageUrl,
               Terrain = d.Terrain.Name,
               IsPublisher = d.PublisherId == userId,
               IsFavorite = favorites != null && favorites.Contains(d.Id),
               FavoritesCount = _context.UsersDestinations.Count(ud => ud.DestinationId == d.Id)
           })
           .ToArrayAsync();
    }

    public async Task<DestinationDeleteDetailsViewModel?> GetDestinationDeleteDetailsReadonlyAsync(int id)
    {
        Destination? d = await _context.Destinations
                .AsNoTracking()
                .Include(d => d.Publisher)
                .FirstOrDefaultAsync(d => d.Id == id);

        if (d is null) return null;

        DestinationDeleteDetailsViewModel vm = new()
        {
            Id = d.Id,
            Name = d.Name,
            PublisherId = d.PublisherId,
            Publisher = d.Publisher.Email!
        };

        return vm;
    }

    public async Task<ICollection<DestinationFavoriteCardViewModel>> GetFavoriteDestinationsReadonlyAsync(string userId) =>
        await _context.UsersDestinations
            .AsNoTracking()
            .Include(ud => ud.Destination)
                .ThenInclude(d => d.Terrain)
            .Where(ud => ud.UserId == userId)
            .Select(ud => new DestinationFavoriteCardViewModel()
            {
                DestinationId = ud.DestinationId,
                ImageUrl = ud.Destination.ImageUrl,
                Name = ud.Destination.Name,
                Terrain = ud.Destination.Terrain.Name
            })
            .ToArrayAsync();

    public async Task<ServiceResult> AddFavoriteDestinationAsync(int destinationId, string userId)
    {
        Destination? d = await _context.Destinations.FirstOrDefaultAsync(d => d.Id == destinationId);
        if (d is null) return new ServiceResult { Found = false };
        if (d.PublisherId == userId) return new ServiceResult { HasPermission = false };

        bool isAlreadyAdded =
            _context.UsersDestinations.Any(ud => ud.UserId == userId && ud.DestinationId == destinationId);
        if (!isAlreadyAdded)
        {
            _context.UsersDestinations.Add(new()
            {
                DestinationId = destinationId,
                UserId = userId
            });

            await _context.SaveChangesAsync();
        }

        return new ServiceResult();
    }

    public async Task<ServiceResult> RemoveFavoriteDestinationAsync(int destinationId, string userId)
    {
        UserDestination? ud = await _context.UsersDestinations
            .FirstOrDefaultAsync(ud => ud.DestinationId == destinationId && ud.UserId == userId);

        if (ud is null) return new ServiceResult() { Found = false };

        _context.UsersDestinations.Remove(ud);
        await _context.SaveChangesAsync();
        return new ServiceResult();
    }
}

