using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Model
{
    public class Society
    {
        [Key]
        public int SocietyId { get; set; }

        [Required(ErrorMessage = "Society Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Primary contact number is required")]
        [Display(Name = "Primary Contact")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number starting with 6-9")]
        public string? Contact { get; set; }

        [Required(ErrorMessage = "Secondary contact number is required")]
        [Display(Name = "Secondary Contact")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number starting with 6-9")]
        [DataType(DataType.PhoneNumber)]
        public string Contact2 { get; set; } = string.Empty;


        public bool IsActive { get; set; }
        public long InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }

}
