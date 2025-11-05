using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Model
{
    public class Member
    {
        [Key]
        public int MemberId { get; set; }
        public int FlatId { get; set; }
        public bool IsChild { get; set; }
        public int ChildAge { get; set; }
        //public string? QRCodeUrl { get; set; }
        //public bool Visited { get; set; }
        //public DateOnly PassDate { get; set; }
        public bool IsGuest { get; set; }
        public bool IsActive { get; set; }
        public long InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        [ForeignKey("FlatId")]
        public virtual Flat? Flat { get; set; }
    }
}
