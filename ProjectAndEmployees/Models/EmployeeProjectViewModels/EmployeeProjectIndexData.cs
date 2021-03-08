using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAndEmployees.Models.EmployeeProjectViewModels
{
    public class EmployeeProjectIndexData
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Employees> Employees { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
