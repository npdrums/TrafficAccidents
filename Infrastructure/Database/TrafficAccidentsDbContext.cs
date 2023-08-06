using Infrastructure.Database.Entities;
using Infrastructure.Database.Interfaces;

using Microsoft.EntityFrameworkCore;

using Npgsql.NameTranslation;

using System.Reflection;

namespace Infrastructure.Database;

public class TrafficAccidentsDbContext : DbContext, ITrafficAccidentsDbContext
{
    public TrafficAccidentsDbContext(DbContextOptions<TrafficAccidentsDbContext> options) : base(options)
    {
    }

    public DbSet<TrafficAccidentDataModel> TrafficAccidents => Set<TrafficAccidentDataModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var translator = new NpgsqlSnakeCaseNameTranslator();

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(translator.TranslateTypeName(entity.GetTableName()!));

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(translator.TranslateMemberName(property.GetColumnName()!));
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(translator.TranslateMemberName(key.GetName()!));
            }

            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(translator.TranslateMemberName(key.GetConstraintName()!));
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(translator.TranslateMemberName(index.GetDatabaseName()!));
            }
        }
    }
}