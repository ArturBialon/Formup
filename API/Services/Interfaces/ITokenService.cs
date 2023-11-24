using API.Models;

namespace API.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Forwarder user);
    }
}
