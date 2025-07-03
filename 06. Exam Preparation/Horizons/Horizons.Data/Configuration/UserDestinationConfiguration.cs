using Horizons.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizons.Data.Configuration;

public class UserDestinationConfiguration : IEntityTypeConfiguration<UserDestination>
{
    public void Configure(EntityTypeBuilder<UserDestination> builder)
    {
        builder
            .HasQueryFilter(ud => !ud.Destination.IsDeleted);
    }
}