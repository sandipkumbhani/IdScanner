using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Model
{
    public class Block
    {
        [Key] 
        public int BlockId { get; set; }
        public int SocietyId {  get; set; }
        public string? BlockNumber { get; set; }
        [ForeignKey("SocietyId")]
        public virtual Society? Society { get; set; }
    }
}
