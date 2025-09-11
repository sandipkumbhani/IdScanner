using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocPass.Domain.Model;

namespace SocPass.Domain.Interface
{
    public interface ILoginRepository
    {
      Task<User?> GetByEmailAsync(string email);
    }
}
