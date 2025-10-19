using backend.Domain.DTO;
using backend.Domain.Entities;
using backend.Domain.Interfaces;
using backend.Domain.ModelViews;
using backend.Infrastructure.Database;

namespace backend.Domain.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _context;

        public UserServices(ApplicationDbContext context)
        {
            _context = context;
        }      

        public User? GetUser(LoginDTO loginDTO)
        {
            var user = _context.Users.Where(u => u.Email == loginDTO.Usuario).FirstOrDefault();

            if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(loginDTO.Senha, user.PasswordHash))
            {
                return null;
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
    }
}
