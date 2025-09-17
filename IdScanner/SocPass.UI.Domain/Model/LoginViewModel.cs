using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Domain.Model
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email Address")]
        public string? EmailId { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }

    public class ResponseToken
    {
        public string Token { get; set; } = string.Empty;
        public long UserId { get; set; }
        public string UserName { get; set; } = "";
        public string? Name { get; set; }
        public string EmailId { get; set; } = "";
        public int UserRoleId { get; set; }
        public string UserRoleName { get; set; } = "";


    }
}
