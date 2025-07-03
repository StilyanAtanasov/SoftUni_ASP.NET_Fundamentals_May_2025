using Horizons.Data;
using Horizons.Services.Core.Contracts;
using Horizons.Web.ViewModels.Terrain;
using Microsoft.EntityFrameworkCore;

namespace Horizons.Services.Core;

public class TerrainService : ITerrainService
{
    private readonly ApplicationDbContext _context;

    public TerrainService(ApplicationDbContext context) => _context = context;

    public async Task<ICollection<TerrainViewModel>> GetAllTerrainTypesReadOnlyAsync() =>
        await _context.Terrains
        .AsNoTracking()
        .Select(t => new TerrainViewModel()
        {
            Id = t.Id,
            Name = t.Name,
        })
        .ToArrayAsync();
}