using Microsoft.EntityFrameworkCore;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;

namespace SocPass.Infrastructure.Repository
{
    public class ForgotPasswordDbRepository : IForgotPasswordDbRepository
    {
        private readonly AppDbContext _context;
        public ForgotPasswordDbRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.users
                .FirstOrDefaultAsync(u => u.EmailId == email);
        }

        public async Task UpdateAsync(User user)
        {
            _context.users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
