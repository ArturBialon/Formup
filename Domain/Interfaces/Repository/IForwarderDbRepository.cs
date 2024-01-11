using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces.Repository
{
    public interface IForwarderDbRepository
    {
        public Task<ICollection<ForwarderResponseDTO>> GetForwarders();
        public Task<ForwarderResponseDTO> GetForwarderById(int id);
        public Task<ForwarderResponseDTO> EditForwarder(ForwarderRequestDTO editedClient);
        public Task<ForwarderResponseDTO> DeleteForwarderById(int id);
    }
}
