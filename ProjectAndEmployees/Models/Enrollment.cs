using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAndEmployees.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }

        public Project Project { get; set; }
        public Employees Employee { get; set; }
    }
}
