using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;

namespace API.Services.Interfaces
{
    public interface IClientDbRepository
    {
        public Task<ICollection<ClientDTO>> GetClients();
        public Task<ClientDTO> GetClientById(int id);
        public Task<string> AddClient(ClientDTO client);
        public Task<string> EditClient(int id, ClientDTO editedClient);
        public Task<string> DeleteClientById(int id);
    }
}
