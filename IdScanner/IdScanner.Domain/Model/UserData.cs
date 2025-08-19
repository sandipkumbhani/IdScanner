using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Domain.Model
{
    public class UserData
    {
        public Guid UserDataId { get; set; }
        public int  CompanyId { get; set; }
        public int DepartmentId { get; set; }
        public string? Name { get; set; }
        public string? Designation { get; set; }
        public string? Mobile_No { get; set; }

    }
}
