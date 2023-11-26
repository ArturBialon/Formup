using Infrastructure.Models;

namespace Application.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Forwarder user);
    }
}
