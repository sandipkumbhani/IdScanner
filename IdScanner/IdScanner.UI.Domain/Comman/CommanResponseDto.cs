using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.UI.Domain.Comman
{
    public class CommanResponseDto
    {
        public int? StatusCode { get; set; }
        public object? Data { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
