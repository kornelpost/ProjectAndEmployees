using Microsoft.EntityFrameworkCore;
using ProjectAndEmployees.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAndEmployees.Data
{
    public class ProjectAndEmployeesContext : DbContext
    {
        public ProjectAndEmployeesContext(DbContextOptions<ProjectAndEmployeesContext> options) : base(options) { }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().ToTable("Project");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Employees>().ToTable("Employee");

            modelBuilder.Entity<Enrollment>()
                .HasKey(c => new { c.EmployeeId, c.ProjectId });
        }
    }
}

