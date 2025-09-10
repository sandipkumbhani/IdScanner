using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Application.Interface
{
    public interface IForgotPasswordService
    {
        Task<User> CheckEmailidAsync(string email);
        Task<User> UpdatePasswordAsync(string email, User modelUsers);
    }
}
