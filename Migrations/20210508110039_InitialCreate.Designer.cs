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
    [Migration("20210508110039_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Stations");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Timetable", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("TrainId")
                        .HasColumnType("int");

                    b.Property<int>("StationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TicketsSold")
                        .HasColumnType("int");

                    b.Property<bool>("TrainDirection")
                        .HasColumnType("bit");

                    b.HasKey("Id", "TrainId", "StationId");

                    b.HasIndex("StationId");

                    b.HasIndex("TrainId");

                    b.ToTable("Timetables");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Train", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RouteId")
                        .HasColumnType("int");

                    b.Property<int>("SeatsQty")
                        .HasColumnType("int");

                    b.Property<int?>("StationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.HasIndex("StationId");

                    b.ToTable("Trains");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.TransitRoute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("RouteId")
                        .HasColumnType("int");

                    b.Property<int>("StationOrder")
                        .HasColumnType("int");

                    b.Property<int>("TransitRouteId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.ToTable("TransitRoutes");
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

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Employee", b =>
                {
                    b.HasOne("DB_s2_1_1.EntityModels.Station", null)
                        .WithMany("Employees")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Route", b =>
                {
                    b.HasOne("DB_s2_1_1.EntityModels.Station", null)
                        .WithMany("Routes")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Timetable", b =>
                {
                    b.HasOne("DB_s2_1_1.EntityModels.Station", null)
                        .WithMany("Timetables")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DB_s2_1_1.EntityModels.Train", null)
                        .WithMany("Timetables")
                        .HasForeignKey("TrainId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Train", b =>
                {
                    b.HasOne("DB_s2_1_1.EntityModels.Route", null)
                        .WithMany("Trains")
                        .HasForeignKey("RouteId");

                    b.HasOne("DB_s2_1_1.EntityModels.Station", null)
                        .WithMany("Trains")
                        .HasForeignKey("StationId");
                });

            modelBuilder.Entity("DB_s2_1_1.EntityModels.TransitRoute", b =>
                {
                    b.HasOne("DB_s2_1_1.EntityModels.Route", "Route")
                        .WithMany("TransitRoutes")
                        .HasForeignKey("RouteId");

                    b.Navigation("Route");
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

            modelBuilder.Entity("DB_s2_1_1.EntityModels.Route", b =>
                {
                    b.Navigation("Trains");

                    b.Navigation("TransitRoutes");
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
                    b.Navigation("Timetables");

                    b.Navigation("Waitings");
                });
#pragma warning restore 612, 618
        }
    }
}
