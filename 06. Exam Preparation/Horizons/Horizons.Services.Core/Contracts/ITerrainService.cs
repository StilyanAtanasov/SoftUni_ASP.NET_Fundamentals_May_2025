using Horizons.Web.ViewModels.Terrain;

namespace Horizons.Services.Core.Contracts;

public interface ITerrainService
{
    Task<ICollection<TerrainViewModel>> GetAllTerrainTypesReadOnlyAsync();
}