using Microsoft.EntityFrameworkCore;
using BarberAppointmentSystem.Models;

namespace BarberAppointmentSystem.Data
{
    public class BarberContext : DbContext
    {
        public BarberContext(DbContextOptions<BarberContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Services> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employee ve Services arasında Many-to-Many ilişki tanımı
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Services)
                .WithMany(s => s.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeServices", // Ara tablonun adı
                    e => e.HasOne<Services>().WithMany().HasForeignKey("ServiceId"), // Foreign Key: Services
                    s => s.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId") // Foreign Key: Employees
                );
        }

    }

}