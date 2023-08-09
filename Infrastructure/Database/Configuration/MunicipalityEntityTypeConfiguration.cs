using Infrastructure.Database.Entities.Enums;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class MunicipalityEntityTypeConfiguration : IEntityTypeConfiguration<MunicipalityDataModel>
{
    public void Configure(EntityTypeBuilder<MunicipalityDataModel> builder)
    {
        builder.ToTable("municipalities");

        builder.HasKey(x => x.MunicipalityId);

        builder.Property(x => x.MunicipalityName).HasMaxLength(150);
        builder.Property(x => x.MunicipalityBorder).HasColumnType("geometry(geometry, 4326)");
    }
}