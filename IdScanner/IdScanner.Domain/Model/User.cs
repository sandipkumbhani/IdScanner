using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Domain.Model
{
    public class User
    {
        [Key]   
        public long UserId { get; set; }
        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(500)]
        public string? EmailId { get; set; }
        public int UserCount { get; set; }
        [StringLength(500)]
        public string? Password { get; set; }
        [NotMapped]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }
        public int UserRoleId { get; set; }
        public bool IsActive { get; set; }
        public long InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? PasswordSalt { get; set; }
        [ForeignKey("UserRoleId")]
        public virtual UserRole? UserRole { get; set; }
    }
}
