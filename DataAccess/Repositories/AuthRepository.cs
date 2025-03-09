using Application;
using Application.Abstractions;
using Domain.Enumerations;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly BlogDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(BlogDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResult> RegisterAsync(string username, string email, string password, UserRole role)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return new AuthResult
                {
                    Success = false,
                    Message = "User already exists"
                };

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password); // Хешируем пароль

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Role = role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResult
            {
                Success = true,
                Message = "User registered successfully"
            };
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return new AuthResult
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            string token = GenerateJwtToken(user);

            return new AuthResult
            {
                Success = true,
                Token = token
            };
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
        new Claim("Rating", user.Rating.ToString())
    };

            // Загружаем ключ из `appsettings.json`
            var signingKey = _configuration["JwtSettings:SigningKey"];

            if (string.IsNullOrEmpty(signingKey))
            {
                throw new Exception("JWT Signing Key is missing from configuration!");
            }

            var keyBytes = Convert.FromBase64String(signingKey);
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audiences:0"] ,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }


}
