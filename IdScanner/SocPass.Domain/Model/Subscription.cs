using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SocPass.Domain.Model
{
    public class Subscription : BaseModel
    {
        public int SubscriptionId { get; set; }
        [Required(ErrorMessage = "Please Select a Society")]
        [DisplayName("Society Name")]
        public int SocietyId { get; set; }
        [Required(ErrorMessage = "Please Select a Subscription Start Date")]
        public DateTime StartFrom { get; set; }
        [Required(ErrorMessage = "Please Select a Subscription Start Date")]
        public DateTime EndTo { get; set; }
        public int AllowNoOfName { get; set; }
        public int AllowNoOfContact { get; set; }
        public int AllowNoOfEmail { get; set; }
        [ForeignKey("SocietyId")]
        public virtual Society? Society { get; set; }

    }
}
