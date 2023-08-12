using Infrastructure.Database.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class SettlementEntityTypeConfiguration : IEntityTypeConfiguration<SettlementDataModel>
{
    public void Configure(EntityTypeBuilder<SettlementDataModel> builder)
    {
        builder.ToTable("settlements");

        builder.HasKey(x => x.SettlementId);

        builder.Property(x => x.SettlementName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.SettlementArea).IsRequired().HasColumnType("geometry(geometry, 4326)");

        builder.HasIndex(x => x.SettlementArea).HasMethod("gist");
    }
}