using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadRack.Shared.Entites
{
    public class Department
    {
        public int Id { get; set; }

        [Display(Name = "Department")]
        [MaxLength(100, ErrorMessage = "Field {0} cannot be longer than {1} characters.")]
        [Required(ErrorMessage = "Field {0} is required.")]
        public string Name { get; set; } = null!;
        public int CollegeId { get; set; }

        public College? college { get; set; }
    }
}
