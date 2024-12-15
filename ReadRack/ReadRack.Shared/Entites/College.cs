using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadRack.Shared.Entites
{
    public class College
    {
        public int Id { get; set; }

        [Display(Name = "College")]
        [MaxLength(100, ErrorMessage = "Field {0} cannot be longer than {1} characters.")]
        [Required(ErrorMessage = "Field {0} is required.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Image")]
        public string? Photo { get; set; }
        public ICollection<Department>? Departments { get; set; }

        [Display(Name = "Departments")]
        public int DepartmentNumber => Departments == null || Departments.Count == 0 ? 0 : Departments.Count;

        public ICollection<User>? Users { get; set; }

    }
}
