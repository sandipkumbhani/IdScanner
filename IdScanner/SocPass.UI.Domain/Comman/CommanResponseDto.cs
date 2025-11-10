using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Domain.Comman
{
    public class CommanResponseDto<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public T Data { get; set; }
    }
}
