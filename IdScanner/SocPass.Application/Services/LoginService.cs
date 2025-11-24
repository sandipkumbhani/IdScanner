using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using SocPass.Application.Interface;
using SocPass.Domain.DTO;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;

namespace SocPass.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IConfiguration _configuration;
        private readonly string _Jwtkey;
        private readonly string _JwtIssuer;
        private readonly string _JwtAudience;
        private readonly int _JwtExpiry;
        private readonly IUserRoleRepository _userRoleRepository;
        public LoginService(ILoginRepository loginRepository, IConfiguration configuration, IUserRoleRepository userRoleRepository)
        {
            _loginRepository = loginRepository;
            _configuration = configuration;
            _Jwtkey = _configuration["Jwt:Key"];
            _JwtIssuer = _configuration["Jwt:Issuer"];
            _JwtAudience = _configuration["Jwt:Audience"];
            _JwtExpiry = int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");
            _userRoleRepository = userRoleRepository;
        }
        public async Task<LoginUserDTO?> LoginAsync(string email, string password)
       {
            try
            {
                var user = await _loginRepository.GetByEmailAsync(email);
                if (user == null || string.IsNullOrEmpty(user.Password))
                {
                    return null;
                }

                if (user.Password != password)
                {
                    return null;
                }

                var role = await _userRoleRepository.GetUserRoleById(user.UserRoleId);

                if (!string.Equals(role.Name, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    var activeSubscription = user.Society?.Subscriptions?
                        .Where(s => s.IsActive)
                        .OrderByDescending(s => s.EndTo)
                        .FirstOrDefault();

                    if (activeSubscription == null || activeSubscription.EndTo < DateTime.UtcNow)
                    {
                        throw new Exception("You don't have an active subscription. Please contact XYZ at mobile number 1234567897.");
                    }
                }
                var token = GenerateJwtToken(user, role.Name);
                return new LoginUserDTO
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    EmailId = user.EmailId,
                    UserRoleName = role.Name,
                    Token = token,
                    Password = password,
                    IsActive = user.IsActive,
                    UpdateBy = user.UpdateBy,
                    UpdateDate = user.UpdateDate,
                    InsertBy = user.InsertBy,
                    InsertDate = user.InsertDate
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                throw;
            }
        }
        private string GenerateJwtToken(User user, string roleName)
        {
            var Cliams = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim("userid", user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.EmailId ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, roleName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Jwtkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _JwtIssuer,
                audience: _JwtAudience,
                claims: Cliams,
                expires: DateTime.Now.AddMinutes(_JwtExpiry),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
