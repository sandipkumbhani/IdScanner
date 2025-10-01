using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public int? SocietyId { get; set; }
        public int UserRoleId { get; set; }
        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(500)]
        public string? EmailId { get; set; }
        [StringLength(500)]
        public string? Password { get; set; }
        [NotMapped]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }
        public bool IsActive { get; set; }
        public long InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        [ForeignKey("SocietyId")]
        public virtual Society? Society { get; set; }
        [ForeignKey("UserRoleId")]
        public virtual UserRole? UserRole { get; set; }
    }
}

