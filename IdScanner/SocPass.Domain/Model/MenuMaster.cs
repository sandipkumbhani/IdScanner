using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Model
{
    public class MenuMaster :BaseModel
    {
        [Key]
        public int MenuId { get; set; }
        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        [StringLength(100)]
        public string? Icon { get; set; }
        public string? Url { get; set; }
        public bool IsDefault { get; set; }
    }
}
