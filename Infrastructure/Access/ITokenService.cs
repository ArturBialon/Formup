using Domain.Models;

namespace Infrastructure.Access
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
