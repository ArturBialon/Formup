using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces.Repository
{
    public interface IRegisterService
    {
        Task<UserResponseDTO> Register(ForwarderRequestDTO forwarder);
    }
}
