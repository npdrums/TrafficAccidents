using Infrastructure.Database.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class CityEntityTypeConfiguration : IEntityTypeConfiguration<CityDataModel>
{
    public void Configure(EntityTypeBuilder<CityDataModel> builder)
    {
        builder.ToTable("cities");

        builder.HasKey(x => x.CityId);

        builder.Property(x => x.CityName).HasMaxLength(150);
        builder.Property(x => x.CityArea).HasColumnType("geometry(geometry, 4326)");

        builder.HasIndex(x => x.CityArea).HasMethod("gist");
    }
}