using System.ComponentModel.DataAnnotations;

namespace SocPass.Domain.Model
{
    public class UserRole : BaseModel
    {
        [Key]
        public int UserRoleId { get; set; }
        [StringLength(200)]
        public string? Name { get; set; }
    }

}
