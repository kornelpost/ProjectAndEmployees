using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAndEmployees.Models.CompanyViewModels
{
    public class ProjectIndexData
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Employees> Employees { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
