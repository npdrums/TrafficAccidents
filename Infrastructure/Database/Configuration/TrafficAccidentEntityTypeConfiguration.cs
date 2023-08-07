using Infrastructure.Database.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class TrafficAccidentEntityTypeConfiguration : IEntityTypeConfiguration<TrafficAccidentDataModel>
{
    public void Configure(EntityTypeBuilder<TrafficAccidentDataModel> builder)
    {
        builder.HasKey(x => x.TrafficAccidentId);

        builder.HasIndex(x => x.PoliceDepartment);

        builder.Property(x => x.ExternalTrafficAccidentId).IsRequired().HasMaxLength(10);
        builder.Property(x => x.PoliceDepartment).IsRequired().HasMaxLength(150);
        builder.Property(x => x.ReportedOn).IsRequired();

        // According to: http://postgis.net/workshops/postgis-intro/geography.html#why-not-use-geography
        // If geometries are compact (country, city, municipality level) Geometry type can be used with appropriate SRID

        builder.Property(x => x.AccidentLocation).HasColumnType("geometry (point, 4326)").IsRequired();
        builder.Property(x => x.ParticipantsStatus).IsRequired();
        builder.Property(x => x.ParticipantsNominalCount).IsRequired();
        builder.Property(x => x.AccidentType).IsRequired();
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(400);
    }
}