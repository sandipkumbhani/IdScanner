using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Domain.Model
{
    public class CompanyMaster
    {
        [Key]
        public int CompanyId { get; set; }
        [StringLength(200)]
        public string? CompanyName { get; set; }
        public string? City { get; set; }
        public string? logo { get; set; }
        public bool IsActive { get; set; }
        public long InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
