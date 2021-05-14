﻿// <auto-generated />
using DB_s2_1_1.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DB_s2_1_1.Migrations
{
    [DbContext(typeof(TrainsContext))]
    [Migration("20210514102017_nrwMigr")]
    partial class nrwMigr
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DB_s2_1_1.EntityModels.TransitRoute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("StationOrder")
                        .HasColumnType("int");

                    b.Property<int>("TransitRouteId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("TransitRoutes");
                });
#pragma warning restore 612, 618
        }
    }
}
