using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadRack.Shared.DTOs
{
    public class PaginationDTO
    {
        public int Id { get; set; }

        public int Page { get; set; }

        public int RecordsNumber { get; set; } = 10;

        public string? Filter { get; set; }

        public string? DepartmentFilter { get; set; }
    }
}
