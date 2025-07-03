namespace Horizons.Web.ViewModels.Destination;

using Horizons.Web.ViewModels.Terrain;
using System.ComponentModel.DataAnnotations;
using static Horizons.GCommon.ValidationConstants.Destination;

public class DestinationFormViewModel
{
	public int Id { get; set; }

	[Required]
	[StringLength(NameMaxLength, MinimumLength = NameMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
	public string Description { get; set; } = null!;

	public string? ImageUrl { get; set; }

	[Required]
	public int TerrainId { get; set; }

	public ICollection<TerrainViewModel>? Terrains { get; set; }

	[Required]
	public string PublishedOn { get; set; } = null!;

	public string? PublisherId { get; set; } = null!;
}