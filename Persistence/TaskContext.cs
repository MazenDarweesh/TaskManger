using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Persistence
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

        public DbSet<TaskDomain> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure primary key
            modelBuilder.Entity<TaskDomain>().HasKey(t => t.Id);

            modelBuilder.Entity<TaskDomain>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        }
    }
}
