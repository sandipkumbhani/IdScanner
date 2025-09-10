using Google.Apis.Drive.v3.Data;
using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = IdScanner.Domain.Model.User;

namespace IdScanner.Domain.Interface
{
    public interface IGetLoginUserNameRepository
    {
        Task<User> GetUserNameAsync(long userid);
    }
}
