namespace Horizons.Web.ViewModels.Destination;

public class DestinationCardViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Terrain { get; set; } = null!;

    public int FavoritesCount { get; set; }

    public bool IsFavorite { get; set; }

    public bool IsPublisher { get; set; }

    public string? ImageUrl { get; set; }
}