using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Domain.Model
{
    public class UserData
    {
        [Key]
        public long UserDataId { get; set; }
        public int CompanyId { get; set; }
        public int DepartmentId { get; set; }
        [StringLength(200)]
        public string? Name { get; set; }
        public string? Designation { get; set; }
        public string? IdNumber { get; set; }
        [Phone]
        public string? MobileNumber { get; set; }
        public string? StallPfNumber { get; set; }
        public string? Licensee { get; set; }
        public string? WorkSlot { get; set; }
        public DateTime? IdValidTill { get; set; }
        public string? PhotoUrl { get; set; }
        public string? PoliceVerificationCertificateUrl { get; set; }
        public string? MedicalCertificateUrl { get; set; }
        public bool IsActive { get; set; }
        public long InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        [ForeignKey("CompanyId")]
        public virtual CompanyMaster? CompanyMaster { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }
      
    }
}
