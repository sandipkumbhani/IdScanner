using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocPass.Domain.Model
{
    public class Member : BaseModel
    {
        [Key]
        public int MemberId { get; set; }
        public int FlatId { get; set; }
        public bool IsChild { get; set; }
        public int ChildAge { get; set; }
        public bool IsGuest { get; set; }
        [ForeignKey("FlatId")]
        public virtual Flat? Flat { get; set; }
    }
}
