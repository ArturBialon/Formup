using API.DTO.Request;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface ILogin
    {
        Task<UserDTO> AddForwarder(ForwarderAddDTO forwarder);
        Task<UserDTO> UserLoginStatus(UserLoginDTO user);
    }
}
