using Domain.Models;

namespace Domain.Interfaces.UserService
{
    public interface ITokenService
    {
        string CreateToken(Forwarder user);
    }
}
