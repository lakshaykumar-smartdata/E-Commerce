using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserService.Data;
using UserService.Enums;
using UserService.Models;
using BC = BCrypt.Net.BCrypt; // For hashing passwords

namespace UserService.Services
{
    public class UserService : IUserService
    {
        private readonly UserServiceDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserService(UserServiceDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        // User Registration with Password Hashing
        public async Task<string> UserRegistration(User user)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == user.Email))
                return "User already exists.";

            user.PasswordHash = BC.HashPassword(user.PasswordHash); // Hash password before saving
            user.CreatedAt = DateTime.UtcNow;
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return "User registered successfully.";
        }

        // User Login - Verify Password and Generate JWT Token
        public async Task<string> UserLogin(string email, string password)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BC.Verify(password, user.PasswordHash))
                return "Invalid email or password.";
            user.LastLoginAt = DateTime.UtcNow;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return GenerateJwtToken(user);
        }

        // Generate JWT Token
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("UserId", user.UserId.ToString()),
                new Claim("UserRoleId", ((int)user.UserRole).ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
