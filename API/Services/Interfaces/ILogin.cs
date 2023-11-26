using Infrastructure.DTO.Request;
using System.Threading.Tasks;

namespace Infrastructure.Services.Interfaces
{
    public interface ILogin
    {
        Task<UserDTO> AddForwarder(ForwarderAddDTO forwarder);
        Task<UserDTO> UserLoginStatus(UserLoginDTO user);
    }
}
