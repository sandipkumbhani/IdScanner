    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Domain.Model
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public int CompanyId { get; set; }
        [StringLength(200)]
        public string? DepartmentName { get; set; }
        [StringLength(100)]
        public string? City { get; set; }
        public bool IsActive { get; set; }
        public long InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        [ForeignKey("CompanyId")]
        public virtual CompanyMaster? CompanyMaster { get; set; }
   
    }
}
