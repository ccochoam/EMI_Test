using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EmployeeDbContext: DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
        }

        // Definimos los DbSets, que representarán las tablas de la base de datos
        public DbSet<Employee> Employees { get; set; }
        public DbSet<PositionHistory> PositionHistories { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Configurar relaciones o restricciones adicionales
            modelBuilder.Entity<Employee>()
                .Ignore(e => e.BonusStrategy)
                .HasMany(e => e.PositionHistories)
                .WithOne()
                .HasForeignKey(ph => ph.EmployeeId);

            modelBuilder.Entity<PositionHistory>()
    .HasKey(ph => ph.Id);
            modelBuilder.Entity<PositionHistory>()
    .Property(ph => ph.Id)
    .ValueGeneratedOnAdd();
            modelBuilder.Entity<PositionHistory>()
    .HasKey(ph => new { ph.EmployeeId, ph.Position, ph.StartDate });


            modelBuilder.Entity<User>()
    .HasKey(ph => ph.Id);

        }
    }
}
