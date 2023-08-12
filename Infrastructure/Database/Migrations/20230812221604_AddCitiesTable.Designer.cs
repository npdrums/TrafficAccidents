﻿// <auto-generated />
using System;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    [DbContext(typeof(TrafficAccidentsDbContext))]
    [Migration("20230812221604_AddCitiesTable")]
    partial class AddCitiesTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Infrastructure.Database.Entities.CityDataModel", b =>
                {
                    b.Property<Guid>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("city_id");

                    b.Property<Geometry>("CityArea")
                        .IsRequired()
                        .HasColumnType("geometry(geometry, 4326)")
                        .HasColumnName("city_area");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("city_name");

                    b.HasKey("CityId")
                        .HasName("pk_cities");

                    b.HasIndex("CityArea")
                        .HasDatabaseName("ix_cities_city_area");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("CityArea"), "gist");

                    b.ToTable("cities", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Database.Entities.MunicipalityDataModel", b =>
                {
                    b.Property<Guid>("MunicipalityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("municipality_id");

                    b.Property<Geometry>("MunicipalityArea")
                        .IsRequired()
                        .HasColumnType("geometry(geometry, 4326)")
                        .HasColumnName("municipality_area");

                    b.Property<string>("MunicipalityName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("municipality_name");

                    b.HasKey("MunicipalityId")
                        .HasName("pk_municipalities");

                    b.HasIndex("MunicipalityArea")
                        .HasDatabaseName("ix_municipalities_municipality_area");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("MunicipalityArea"), "gist");

                    b.ToTable("municipalities", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Database.Entities.SettlementDataModel", b =>
                {
                    b.Property<Guid>("SettlementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("settlement_id");

                    b.Property<Geometry>("SettlementArea")
                        .IsRequired()
                        .HasColumnType("geometry(geometry, 4326)")
                        .HasColumnName("settlement_area");

                    b.Property<string>("SettlementName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("settlement_name");

                    b.HasKey("SettlementId")
                        .HasName("pk_settlements");

                    b.HasIndex("SettlementArea")
                        .HasDatabaseName("ix_settlements_settlement_area");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SettlementArea"), "gist");

                    b.ToTable("settlements", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Database.Entities.TrafficAccidentDataModel", b =>
                {
                    b.Property<Guid>("TrafficAccidentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("traffic_accident_id");

                    b.Property<Point>("AccidentLocation")
                        .IsRequired()
                        .HasColumnType("geometry (point, 4326)")
                        .HasColumnName("accident_location");

                    b.Property<int>("AccidentType")
                        .HasColumnType("integer")
                        .HasColumnName("accident_type");

                    b.Property<Guid?>("CityId")
                        .HasColumnType("uuid")
                        .HasColumnName("city_id");

                    b.Property<string>("Description")
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)")
                        .HasColumnName("description");

                    b.Property<string>("ExternalTrafficAccidentId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("external_traffic_accident_id");

                    b.Property<Guid?>("MunicipalityId")
                        .HasColumnType("uuid")
                        .HasColumnName("municipality_id");

                    b.Property<int>("ParticipantsNominalCount")
                        .HasColumnType("integer")
                        .HasColumnName("participants_nominal_count");

                    b.Property<int>("ParticipantsStatus")
                        .HasColumnType("integer")
                        .HasColumnName("participants_status");

                    b.Property<string>("PoliceDepartment")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("police_department");

                    b.Property<DateTime>("ReportedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("reported_on");

                    b.Property<Guid?>("SettlementId")
                        .HasColumnType("uuid")
                        .HasColumnName("settlement_id");

                    b.HasKey("TrafficAccidentId")
                        .HasName("pk_traffic_accidents");

                    b.HasIndex("AccidentLocation")
                        .HasDatabaseName("ix_traffic_accidents_accident_location");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("AccidentLocation"), "gist");

                    b.HasIndex("AccidentType")
                        .HasDatabaseName("ix_traffic_accidents_accident_type");

                    b.HasIndex("CityId")
                        .HasDatabaseName("ix_traffic_accidents_city_id");

                    b.HasIndex("MunicipalityId")
                        .HasDatabaseName("ix_traffic_accidents_municipality_id");

                    b.HasIndex("ParticipantsNominalCount")
                        .HasDatabaseName("ix_traffic_accidents_participants_nominal_count");

                    b.HasIndex("ParticipantsStatus")
                        .HasDatabaseName("ix_traffic_accidents_participants_status");

                    b.HasIndex("PoliceDepartment")
                        .HasDatabaseName("ix_traffic_accidents_police_department");

                    b.HasIndex("SettlementId")
                        .HasDatabaseName("ix_traffic_accidents_settlement_id");

                    b.ToTable("traffic_accidents");
                });

            modelBuilder.Entity("Infrastructure.Database.Entities.TrafficAccidentDataModel", b =>
                {
                    b.HasOne("Infrastructure.Database.Entities.CityDataModel", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .HasConstraintName("fk_traffic_accidents_cities_city_id");

                    b.HasOne("Infrastructure.Database.Entities.MunicipalityDataModel", "Municipality")
                        .WithMany()
                        .HasForeignKey("MunicipalityId")
                        .HasConstraintName("fk_traffic_accidents_municipalities_municipality_id");

                    b.HasOne("Infrastructure.Database.Entities.SettlementDataModel", "Settlement")
                        .WithMany()
                        .HasForeignKey("SettlementId")
                        .HasConstraintName("fk_traffic_accidents_settlements_settlement_id");

                    b.Navigation("City");

                    b.Navigation("Municipality");

                    b.Navigation("Settlement");
                });
#pragma warning restore 612, 618
        }
    }
}
