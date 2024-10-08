﻿using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Domain.Entities;

namespace Persistence
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

        public DbSet<TaskDomain> TaskDomains { get; set; }
        public DbSet<Student> Students { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            var ulidConverter = new ValueConverter<Ulid, string>(
            v => v.ToString(),
            v => Ulid.Parse(v));

           // Apply the converter to Student entity
            modelBuilder.Entity<Student>()
                .Property(e => e.Id)
                .HasConversion(ulidConverter)
                .ValueGeneratedOnAdd();
           
            modelBuilder.Entity<TaskDomain>()
                .Property(e => e.Id)
                .HasConversion(ulidConverter)
                .ValueGeneratedOnAdd();


            // Configure the one-to-many relationship
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Tasks)
                .WithOne(t => t.Student)
                .HasForeignKey(t => t.StudentId);

            base.OnModelCreating(modelBuilder);


        }
    }
}
