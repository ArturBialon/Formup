using Domain.DTO;
using Infrastructure.Models;

namespace Domain.Interfaces.Repository
{
    public interface IClientDbRepository
    {
        public Task<ICollection<ClientDTO>> GetClients();
        public Task<Client> GetClientById(Client.EntityId id);
        public Task<Client> GetClientByTax(string tax);
        public Task<Client> GetDuplicatedClient(string tax, Client.EntityId id);
        public Task<bool> AddClient(ClientDTO client);
        public Task<bool> EditClient(ClientDTO editedClient, Client contactorFromDB);
        public Task<bool> DeleteClient(Client contactorFromDB);
    }
}
