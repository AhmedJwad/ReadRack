﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReadRack.Shared.Entites;

namespace ReadRack.Backend.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) 
        {
                
        }

        public DbSet<College> colleges { get; set; }      
        public DbSet<Department>departments { get; set; }   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<College>().HasIndex(c=>c.Name).IsUnique();
            modelBuilder.Entity<Department>().HasIndex(c => c.Name).IsUnique();
        }
    }
}
