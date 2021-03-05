using ProjectAndEmployees.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAndEmployees.Data
{
    public class DbInitializer
    {
        public static void Initialize(ProjectAndEmployeesContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Employees.Any())
            {
                return;   // DB has been seeded
            }

            var employee = new Employees[]
            {
            new Employees{FirstName="Carson",LastName="Alexander",ReleaseDate=DateTime.Parse("2005-09-01")},
            new Employees{FirstName="Meredith",LastName="Alonso",ReleaseDate=DateTime.Parse("2002-09-01")},
            new Employees{FirstName="Arturo",LastName="Anand",ReleaseDate=DateTime.Parse("2003-09-01")},
            new Employees{FirstName="Gytis",LastName="Barzdukas",ReleaseDate=DateTime.Parse("2002-09-01")},
            new Employees{FirstName="Yan",LastName="Li",ReleaseDate=DateTime.Parse("2002-09-01")},
            new Employees{FirstName="Peggy",LastName="Justice",ReleaseDate=DateTime.Parse("2001-09-01")},
            new Employees{FirstName="Laura",LastName="Norman",ReleaseDate=DateTime.Parse("2003-09-01")},
            new Employees{FirstName="Nino",LastName="Olivetto",ReleaseDate=DateTime.Parse("2005-09-01")}
            };
            foreach (Employees s in employee)
            {
                context.Employees.Add(s);
            }
            context.SaveChanges();

            var project = new Project[]
            {
            new Project{ProjectId=1050,Title="Chemistry",Description="test"},
            new Project{ProjectId=4022,Title="Microeconomics",Description="test"},
            new Project{ProjectId=4041,Title="Macroeconomics",Description="test"},
            new Project{ProjectId=1045,Title="Calculus",Description="test"},
            new Project{ProjectId=3141,Title="Trigonometry",Description="test"},
            new Project{ProjectId=2021,Title="Composition",Description="test"},
            new Project{ProjectId=2042,Title="Literature",Description="test"}
            };
            foreach (Project c in project)
            {
                context.Projects.Add(c);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
            new Enrollment{EmployeeId=1,ProjectId=1050},
            new Enrollment{EmployeeId=1,ProjectId=4022},
            new Enrollment{EmployeeId=1,ProjectId=4041},
            new Enrollment{EmployeeId=2,ProjectId=1045},
            new Enrollment{EmployeeId=2,ProjectId=3141},
            new Enrollment{EmployeeId=2,ProjectId=2021},
            new Enrollment{EmployeeId=3,ProjectId=1050},
            new Enrollment{EmployeeId=4,ProjectId=1050},
            new Enrollment{EmployeeId=4,ProjectId=4022},
            new Enrollment{EmployeeId=5,ProjectId=4041},
            new Enrollment{EmployeeId=6,ProjectId=1045},
            new Enrollment{EmployeeId=7,ProjectId=3141},
            };
            foreach (Enrollment e in enrollments)
            {
                context.Enrollments.Add(e);
            }
            context.SaveChanges();
        }
    }
}