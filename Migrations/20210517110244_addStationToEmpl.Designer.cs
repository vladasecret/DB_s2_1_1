﻿// <auto-generated />
using System;
using DB_s2_1_1.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DB_s2_1_1.Migrations
{
    [DbContext(typeof(TrainsContext))]
    [Migration("20210517110244_addStationToEmpl")]
    partial class addStationToEmpl
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Speed")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FIO")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.Property<int>("StationId")
                        .HasColumnType("int");

                    b.Property<int>("StationOrder")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.HasIndex("RouteId", "StationId")
                        .IsUnique();

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Stations");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Timetable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoadId")
                        .HasColumnType("int");

                    b.Property<int>("StationId")
                        .HasColumnType("int");

                    b.Property<bool>("TrainDirection")
                        .HasColumnType("bit");

                    b.Property<int>("TrainId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.HasIndex("TrainId");

                    b.HasIndex("Id", "TrainId", "StationId")
                        .IsUnique();

                    b.ToTable("Timetables");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Train", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.Property<int>("SeatsQty")
                        .HasColumnType("int");

                    b.Property<int?>("StationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("StationId");

                    b.ToTable("Trains");
                });

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

            modelBuilder.Entity("DB_s2_1_1.EntityModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecretPhrase")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Waiting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("DelayMinutes")
                        .HasColumnType("int");

                    b.Property<bool>("TrainDirection")
                        .HasColumnType("bit");

                    b.Property<int>("TrainId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TrainId");

                    b.ToTable("Waitings");
                });

            modelBuilder.Entity("EmployeeTrain", b =>
                {
                    b.Property<int>("EmployeesId")
                        .HasColumnType("int");

                    b.Property<int>("TrainsId")
                        .HasColumnType("int");

                    b.HasKey("EmployeesId", "TrainsId");

                    b.HasIndex("TrainsId");

                    b.ToTable("EmployeeTrain");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("RoleUser");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Employee", b =>
                {
                    b.HasOne("DB_s2_1_1.EntityModels.Station", "Station")
                        .WithMany("Employees")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Station");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Route", b =>
                {
                    b.HasOne("DB_s2_1_1.EntityModels.Station", "Station")
                        .WithMany("Routes")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Station");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Timetable", b =>
                {
                    b.HasOne("DB_s2_1_1.EntityModels.Station", null)
                        .WithMany("Timetables")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DB_s2_1_1.EntityModels.Train", "Train")
                        .WithMany()
                        .HasForeignKey("TrainId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Train");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Train", b =>
                {
                    b.HasOne("DB_s2_1_1.EntityModels.Category", "Category")
                        .WithMany("Trains")
                        .HasForeignKey("CategoryId");

                    b.HasOne("DB_s2_1_1.EntityModels.Station", "Station")
                        .WithMany("Trains")
                        .HasForeignKey("StationId");

                    b.Navigation("Category");

                    b.Navigation("Station");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Waiting", b =>
                {
                    b.HasOne("DB_s2_1_1.EntityModels.Train", null)
                        .WithMany("Waitings")
                        .HasForeignKey("TrainId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EmployeeTrain", b =>
                {
                    b.HasOne("DB_s2_1_1.EntityModels.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DB_s2_1_1.EntityModels.Train", null)
                        .WithMany()
                        .HasForeignKey("TrainsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("DB_s2_1_1.EntityModels.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DB_s2_1_1.EntityModels.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Category", b =>
                {
                    b.Navigation("Trains");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Station", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Routes");

                    b.Navigation("Timetables");

                    b.Navigation("Trains");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Train", b =>
                {
                    b.Navigation("Waitings");
                });
#pragma warning restore 612, 618
        }
    }
}
