using Microsoft.EntityFrameworkCore;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;

namespace SocPass.Infrastructure.Repository
{
    public class GetLoginUserNameRepository : IGetLoginUserNameRepository
    {
        private readonly AppDbContext _context;
        public GetLoginUserNameRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<User> GetUserNameAsync(int userid)
        {
            var userName = await _context.users
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
