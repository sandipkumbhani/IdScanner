using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Model
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public int SocietyId { get; set; }
        [Required(ErrorMessage = "Event name is required")]
        [MaxLength(150)]
        public string? EventName { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [MaxLength(250)]
        public string? Location { get; set; }

        [MaxLength(100)]
        public string? Organizer { get; set; }
        public bool IsActive { get; set; }
        public long InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        [ForeignKey("SocietyId")]
        public virtual Society? Society { get; set; }
    }
}
