using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SocPass.Domain.Model
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public int SocietyId { get; set; }
        public DateTime StartFrom { get; set; }
        public DateTime EndTo { get; set; }
        public int AllowNoOfName { get; set; }
        public int AllowNoOfContact { get; set; }
        public int AllowNoOfEmail { get; set; }
        public bool IsActive { get; set; }
        public long InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        [ForeignKey("SocietyId")]
        public virtual Society? Society { get; set; }

    }
}
