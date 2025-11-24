using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Model
{
    public class Flat : BaseModel
    {
        [Key]
        public int FlatId { get; set; }
        [DisplayName("Society Name")]
        public int SocietyId { get; set; }
        public int BlockId { get; set; }
        public int NumberOfFlats { get; set; }
        public string? FlatNumber { get; set; }
        [NotMapped]
        public int StartFlatNumber { get; set; }

        [NotMapped]
        public int EndFlatNumber { get; set; }

        //[NotMapped]
        //public int FlatsPerFloor { get; set; }
        public int FloorNumber { get; set; }
        public int TotalMember { get; set; }
        public int NumberOfAdult { get; set; }
        public int NumberOfChild { get; set; }
        [ForeignKey("SocietyId")]
        public virtual Society? Society { get; set; }
        [ForeignKey("BlockId")]
        public virtual Block? Block { get; set; }
        public ICollection<Member> Members { get; set; } = new List<Member>();

    }
}
