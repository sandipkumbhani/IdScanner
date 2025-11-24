using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SocPass.Domain.Model
{
    public class Subscription : BaseModel
    {
        public int SubscriptionId { get; set; }
        public int SocietyId { get; set; }
        public DateTime StartFrom { get; set; }
        public DateTime EndTo { get; set; }
        public int AllowNoOfName { get; set; }
        public int AllowNoOfContact { get; set; }
        public int AllowNoOfEmail { get; set; }
        [ForeignKey("SocietyId")]
        public virtual Society? Society { get; set; }

    }
}
