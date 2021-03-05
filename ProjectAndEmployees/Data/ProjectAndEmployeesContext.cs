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
    }
}

