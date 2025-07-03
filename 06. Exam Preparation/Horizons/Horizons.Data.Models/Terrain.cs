using System.ComponentModel.DataAnnotations;
using static Horizons.GCommon.ValidationConstants.Terrain;

namespace Horizons.Data.Models;

public class Terrain
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    public ICollection<Destination> Destinations { get; set; } = new HashSet<Destination>();
}