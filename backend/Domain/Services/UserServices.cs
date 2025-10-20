using backend.Domain.DTO;
using backend.Domain.Entities;
using backend.Domain.Exceptions;
using backend.Domain.Interfaces;
using backend.Domain.ModelViews;
using backend.Infrastructure.Database;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Domain.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserServices(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }      

        public User? GetUser(LoginDTO loginDTO)
        {
            var user = _context.Users.Where(u => u.Email == loginDTO.Usuario).FirstOrDefault();

            if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(loginDTO.Senha, user.PasswordHash))
            {
                throw new UnauthorizedException("Dados de login inválidos.");
            }

            return user;
        }

        public UserView GetUserView(User user)
        {
            return new UserView()
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
            };
        }

        public string GenerateTokenJwt(UserView user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt").GetSection("Key").Value ?? "5uJAjTCGY%YD82e7NF!gW3xZ$TC57M8F"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
