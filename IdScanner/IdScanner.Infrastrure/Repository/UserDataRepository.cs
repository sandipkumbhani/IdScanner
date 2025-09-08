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
        public async Task<UserData> GetUserDataById(int id)
        {
            return await _context.UserDatas
                .Include(e => e.CompanyMaster)
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.UserDataId == id && e.IsActive);
        }
		public async Task UpdateQrCodeAsync(long userId, string qrCodeUrl)
		{
			var user = await _context.UserDatas.FindAsync(userId);
			if (user != null)
			{
				user.QRCodeUrl = qrCodeUrl;
				_context.UserDatas.Update(user);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<List<UserData>> GetAllUserDataAsync()
        {
            return await _context.UserDatas.Include(u => u.CompanyMaster)
                .Include(u => u.Department)
                .Where(u => u.IsActive).ToListAsync();
        }
        public async Task UpdateUserDataAsync(UserData userData)
        {
            _context.UserDatas.Update(userData);
            _context.SaveChanges();
        }
        public async Task DeleteAsync(UserData userData)
        {
            var userExisting = await _context.UserDatas.FindAsync(userData.UserDataId);
            if (userExisting != null)
            {
                userExisting.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
