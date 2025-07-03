using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizons.Data.Models.Configuration;

public class DestinationConfiguration : IEntityTypeConfiguration<Destination>
{
    public void Configure(EntityTypeBuilder<Destination> builder)
    {
        builder
            .Property(d => d.IsDeleted)
            .HasDefaultValue(false);

        builder
            .HasOne(d => d.Terrain)
            .WithMany(t => t.Destinations)
            .HasForeignKey(d => d.TerrainId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(d => d.Publisher)
            .WithMany()
            .HasForeignKey(d => d.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasQueryFilter(d => !d.IsDeleted);
    }
}

