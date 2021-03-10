using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAndEmployees.Models
{
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProjectId { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
