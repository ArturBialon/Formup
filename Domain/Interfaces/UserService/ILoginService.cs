using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces.UserAccessService
{
    public interface ILoginService
    {
        Task<UserResponseDTO> AddForwarder(ForwarderRequestDTO forwarder);
        Task<UserResponseDTO> UserLoginStatus(UserLoginDTO user);
    }
}
