using Domain.DTO.Request;
using Domain.DTO.Response;
using Infrastructure.Models;

namespace Domain.Interfaces.Repository
{
    public interface IForwarderDbRepository
    {
        public Task<ICollection<ForwarderResponseDTO>> GetForwarders();
        public Task<Forwarder> GetForwarderById(int id);
        public Task<Forwarder> GetDuplicatedForwarder(ForwarderRequestDTO duplicatedForwarder);
        public Task<bool> EditForwarder(ForwarderRequestDTO forwarderToEdit, Forwarder forwarderFromDB);
        public Task<bool> DeleteForwarder(Forwarder forwarderToDelete);
    }
}
