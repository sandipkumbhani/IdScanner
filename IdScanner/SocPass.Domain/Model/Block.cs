using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocPass.Domain.Model
{
    public class Block
    {
        [Key]
        public int BlockId { get; set; }

        [Required(ErrorMessage = "Please select a society")]
        public int SocietyId { get; set; }

        [Required(ErrorMessage = "Block Number is required")]
        public string? BlockNumber { get; set; }

        public bool IsActive { get; set; }
        public long InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        [ForeignKey("SocietyId")]
        public virtual Society? Society { get; set; }
    }
}
