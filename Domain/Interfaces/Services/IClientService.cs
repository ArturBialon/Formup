using Domain.DTO;

namespace Domain.Interfaces.Services
{
    public interface IClientService
    {
        public Task<ICollection<ClientDTO>> GetClients();
        public Task<ClientDTO> GetClientById(int id);
        public Task<ClientDTO> AddClient(ClientDTO client);
        public Task<ClientDTO> EditClient(ClientDTO editedClient);
        public Task<bool> DeleteClientById(int id);
    }
}
