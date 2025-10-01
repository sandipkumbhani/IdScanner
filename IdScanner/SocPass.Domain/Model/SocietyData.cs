using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Model
{
    public class SocietyData
    {
        public int? SocietyDataId { get; set; }
        public int FlatId { get; set; }
        public string? ContactName { get; set; }
        public string? ContactNumber { get; set; }
        public string? ContactEmail { get; set; }
        public bool IsActive { get; set; }
        public long InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        [ForeignKey("FlatId")]
        public virtual Flat? Flat { get; set; }
    }
}
