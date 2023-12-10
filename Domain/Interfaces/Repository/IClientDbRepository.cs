using Domain.DTO;

namespace Domain.Interfaces.Repository
{
    public interface IClientDbRepository
    {
        public Task<ICollection<ClientDTO>> GetClients();
        public Task<ClientDTO> GetClientById(int id);
        public Task<ClientDTO> AddClient(ClientDTO client);
        public Task<ClientDTO> EditClient(int id, ClientDTO editedClient);
        public Task<ClientDTO> DeleteClientById(int id);
    }
}
