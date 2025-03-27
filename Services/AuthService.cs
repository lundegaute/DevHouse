using DevHouse.Models;
using DevHouse.Data;
using DevHouse.Helper;
using DevHouse.JwtConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace DevHouse.Services {
    public class AuthService {
        private readonly DataContext _context;
        private readonly JwtSettings _jwtSettings;
        public AuthService(DataContext context, JwtSettings jwtSettings) {
            _context = context;
            _jwtSettings = jwtSettings;
        }
        public async Task<bool> ValidateUserAsync(string username, string password) {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if ( user is null ) {
                return false;
            }
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
        public async Task<bool> RegisterUserAsync( string username, string password) {
            await _context.Database.EnsureCreatedAsync();
            var userExists = await _context.Users.AnyAsync(u => u.Username == username);
            if ( userExists ) {
                return false;
            }
            var passwordHasher = new PasswordHasher<User>();
            var newUser = new User {
                Username = username,
                PasswordHash = passwordHasher.HashPassword(null, password)
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<User> GetUser(string username) {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            ValidationHelper.CheckIfExistsOrException((user, nameof(User)));
            return user;
        }
        public string GenerateToken(User user) {
            var claims = new [] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpireMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}