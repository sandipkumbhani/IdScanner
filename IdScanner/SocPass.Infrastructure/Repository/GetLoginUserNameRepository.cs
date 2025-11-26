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
        public async Task<User> GetUserNameAsync(int userId)
        {
            var result = await _context.users
                .Where(u => u.UserId == userId)
                .Select(u => new
                {
                    u.Name,
                    EndTo = u.Society.Subscriptions
                        .OrderByDescending(s => s.EndTo)
                        .Select(s => s.EndTo)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            string subscriptionMessage = " ";

            if (result != null)
            {
                if (result.EndTo != default)
                {
                    int remainingDays = (result.EndTo.Date - DateTime.Now.Date).Days;

                    if (remainingDays < 0)
                        subscriptionMessage = "Subscription expired!";
                    else if (remainingDays <= 2)
                        subscriptionMessage = $"Subscription expires in {remainingDays} day's!";
                    //else
                    //    subscriptionMessage = $"Subscription active until {result.EndTo:dd-MMM-yyyy}";
                }

                return new User
                {
                    UserId = userId,
                    Name = result.Name,
                    SubscriptionMessage = subscriptionMessage
                };
            }

            return null;
        }


    }
}
