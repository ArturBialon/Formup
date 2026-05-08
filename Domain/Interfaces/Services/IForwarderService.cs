using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces.Services
{
    public interface IForwarderService
    {
        public Task<ICollection<ForwarderResponseDTO>> GetForwarders();
        public Task<ForwarderResponseDTO> GetForwarderById(Guid id);
        public Task<bool> EditForwarder(ForwarderRequestDTO forwarderToEdit);
        public Task<bool> DeleteForwarderById(Guid id);
    }
}
