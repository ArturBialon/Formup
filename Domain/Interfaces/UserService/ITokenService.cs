using Infrastructure.Models;

namespace Domain.Interfaces.UserAccessService
{
    public interface ITokenService
    {
        string CreateToken(Forwarder user);
    }
}
