using Infrastructure.Models;

namespace Infrastructure.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Forwarder user);
    }
}
