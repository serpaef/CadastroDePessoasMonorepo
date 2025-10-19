using backend.Domain.DTO;
using backend.Domain.ModelViews;
using backend.Domain.Entities;

namespace backend.Domain.Interfaces
{
    public interface IUserServices
    {
        User? GetUser(LoginDTO loginDTO);

        UserView GetUserView(User user);
    }
}
