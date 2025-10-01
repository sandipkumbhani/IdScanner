using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.DTO
{
    public class SocietyDataCreateRequest
    {
        public int FlatId { get; set; }
        public int SocietyDataId { get; set; }
        public int AllowedFields { get; set; } 
        public List<string> ContactName { get; set; } = new List<string>();
        public List<string> ContactNumber { get; set; } = new List<string>();
        public List<string> ContactEmail { get; set; } = new List<string>();    
        public bool IsActive { get; set; }
        public long InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
