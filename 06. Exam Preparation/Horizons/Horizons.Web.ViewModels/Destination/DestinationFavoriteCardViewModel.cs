namespace Horizons.Web.ViewModels.Destination;

public class DestinationFavoriteCardViewModel
{
    public int DestinationId { get; set; }

    public string Name { get; set; } = null!;

    public string Terrain{ get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;
}