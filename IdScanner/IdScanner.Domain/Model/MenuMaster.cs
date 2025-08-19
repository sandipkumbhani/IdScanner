using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Domain.Model
{
    public class MenuMaster
    {
        [Key]
        public long MenuId { get; set; }
        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        [StringLength(100)]
        public string? Icon { get; set; }
        public string? Url { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

        public long InsertBy { get; set; }

        public DateTime InsertDate { get; set; }

        public long UpdateBy { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
