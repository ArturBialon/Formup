using Domain.DTO;
using Infrastructure.Models;

namespace Domain.Interfaces.Repository
{
    public interface IClientDbRepository
    {
        public Task<ICollection<ClientDTO>> GetClients();
        public Task<Client> GetClientById(int id);
        public Task<Client> GetClientByTax(string tax);
        public Task<Client> GetDuplicatedClient(string tax, int id);
        public Task<bool> AddClient(ClientDTO client);
        public Task<bool> EditClient(ClientDTO editedClient, Client contactorFromDB);
        public Task<bool> DeleteClient(Client contactorFromDB);
    }
}
