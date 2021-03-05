using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAndEmployees.Models
{
    public class Employees
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [RegularExpression(@"^[A-Z]+[a-z]*$")]
        [Required]
        [StringLength(30, MinimumLength = 3)]

        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[A-Z]+[a-z]*$")]
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string LastName { get; set; }

        [Display(Name = "Join Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
