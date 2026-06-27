using Domain.Models;

namespace Domain.Interfaces.UserService
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
