using IdScanner.Domain.Interface;
using IdScanner.Domain.Model;
using IdScanner.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Infrastructure.Repository
{
    public class UserDataRepository : IUserDataRepository
    {
        private AppDbContext _context;
        public UserDataRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<UserData> AddUserDataAsync(UserData UserData)
        {
            _context.UserDatas.Add(UserData);
            await _context.SaveChangesAsync();
            return UserData;
        }
    }
}
