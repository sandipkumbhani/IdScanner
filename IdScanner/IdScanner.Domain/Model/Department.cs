using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Domain.Model
{
    public class Department
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string? DepartmentName { get; set; }
        public bool IsActive { get; set; }
        public long InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

    }
}
