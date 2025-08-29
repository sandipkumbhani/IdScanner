using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace IdScanner.UI.Domain.Helper
{
    public class ApplicationURL
    {
        public string? url { get; set; }
        public ApplicationURL(IConfiguration configuration)
        {
            var qurtzsetting = configuration.GetSection("ApplicationUrl");
            url = qurtzsetting.GetSection("url").Value;
        }
    }
}
