using System.ComponentModel.DataAnnotations.Schema;

namespace SocPass.Domain.Model
{
    public class SocietyData : BaseModel
    {
        public int? SocietyDataId { get; set; }
        public int FlatId { get; set; }
        public string? ContactName { get; set; }
        public string? ContactNumber { get; set; }
        public string? ContactEmail { get; set; }
        [ForeignKey("FlatId")]
        public virtual Flat? Flat { get; set; }
    }
}
