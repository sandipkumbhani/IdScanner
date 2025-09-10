using IdScanner.Domain.Interface;
using IdScanner.Domain.Model;
using IdScanner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Infrastructure.Repository
{
    public class GetLoginUserNameRepository : IGetLoginUserNameRepository
    {
        private readonly AppDbContext _context;
        public GetLoginUserNameRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<User> GetUserNameAsync(long userid)
        {
            var userName = await _context.Users
            .Where(u => u.UserId == userid)
            .Select(u => u.Name)
            .FirstOrDefaultAsync();
            return new User
            {
                UserId = userid,
                Name = userName
            };
        }

    }
}
