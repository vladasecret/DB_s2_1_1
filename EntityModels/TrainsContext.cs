﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.EntityModels
{
    public class TrainsContext : DbContext
    {
        public TrainsContext(DbContextOptions<TrainsContext> options) : base(options)
        {
        }

        public DbSet<TransitRoute> TransitRoutes { get; set; }
        
        public DbSet<Route> Routes { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Timetable> Timetables{ get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<Waiting> Waitings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Timetable>()
               .HasIndex(e => new { e.Id, e.TrainId, e.StationId })
               .IsUnique();
            
            modelBuilder.Entity<Route>()
              .HasIndex(e => new { e.RouteId, e.StationId })
                .IsUnique();
            modelBuilder.Entity<Category>()
                .HasIndex(e => e.Name)
                .IsUnique();



        }

    }


}