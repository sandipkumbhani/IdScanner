using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocPass.Domain.DTO;

namespace SocPass.Application.Interface
{
    public interface ILoginService
    {
        Task<LoginUserDTO?> LoginAsync(string email, string password);
    }
}
