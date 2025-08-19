using IdScanner.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Application.Interface
{
    public interface IUserServices
    {
        Task<LoginUserDTO?> LoginAsync(string email, string password);
    }
}
