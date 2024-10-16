using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Configurar relaciones o restricciones adicionales
            modelBuilder.Entity<Employee>()
                .Ignore(e => e.BonusStrategy)
                .HasMany(e => e.PositionHistories)
                .WithOne()
                .HasForeignKey(ph => ph.EmployeeId);

            modelBuilder.Entity<Employee>()
    .HasOne(e => e.Department)  // Un empleado tiene un departamento
    .WithMany(d => d.Employees)  // Un departamento tiene muchos empleados
    .HasForeignKey(e => e.DepartmentId)  // Clave foránea en Employee
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
    .HasOne(p => p.Department)
    .WithMany(d => d.Projects)
    .HasForeignKey(p => p.DepartmentId);

            modelBuilder.Entity<PositionHistory>()
    .HasKey(ph => ph.Id);
            modelBuilder.Entity<PositionHistory>()
    .Property(ph => ph.Id)
    .ValueGeneratedOnAdd();
            modelBuilder.Entity<PositionHistory>()
    .HasKey(ph => new { ph.EmployeeId, ph.Position, ph.StartDate });

            modelBuilder.Entity<PositionHistory>()
    .HasOne(p => p.Department)
    .WithMany()
    .HasForeignKey(p => p.DepartmentId);

            modelBuilder.Entity<Department>()
                .HasKey(ph => new { ph.Id });

            modelBuilder.Entity<Project>()
                .HasKey(ph => new { ph.Id });

            modelBuilder.Entity<User>()
    .HasKey(ph => ph.Id);

        }
    }
}
