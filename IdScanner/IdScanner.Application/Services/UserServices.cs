using IdScanner.Application.Interface;
using IdScanner.Domain.DTO;
using IdScanner.Domain.Interface;
using IdScanner.Domain.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userLoginRepository;
        private readonly IConfiguration _configuration;
        private readonly string _JwtKey;
        private readonly string _JwtIssuer;
        private readonly string _JwtAudience;
        private readonly int _JwtExpiry;

        public UserServices(IUserRepository userLoginRepository,IConfiguration configuration)
        {
            _userLoginRepository = userLoginRepository;
            _configuration = configuration;
            _JwtKey = _configuration["Jwt:Key"];
            _JwtIssuer = _configuration["Jwt:Issuer"];
            _JwtAudience = _configuration["Jwt:Audience"];
            _JwtExpiry = int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");
        }
        public async Task<LoginUserDTO?> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _userLoginRepository.GetByEmailAsync(email);

                if (user == null || string.IsNullOrEmpty(user.Password))
                {
                    return null;
                }
                //var role = await _userLoginRepository.GetUserWithRoleAsync(user.UserRoleId);
                if (user.Password != password)
                {
                    return null;
                }

                var token = GenrateJWTToken(user);


                return new LoginUserDTO
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    EmailId = user.EmailId,
                    Token = token,
                    Password = password,
                    UserRoleId = user.UserRoleId,
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
            }
            return null;
        }
        private string GenrateJWTToken(User users)
        {
            var Claims = new[]
            {
                  new Claim(JwtRegisteredClaimNames.Sub, users.UserId.ToString()),
                   new Claim(JwtRegisteredClaimNames.Email, users.EmailId ?? string.Empty),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                 issuer: _JwtIssuer,
                 audience: _JwtAudience,
                claims: Claims,
                expires: DateTime.Now.AddMinutes(_JwtExpiry),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
