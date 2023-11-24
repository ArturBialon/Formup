using API.DTO;
using API.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface IClientDbRepository
    {
        public Task<ICollection<ClientDTO>> GetClients();
        public Task<ClientDTO> GetClientById(int id);
        public Task<CommonEnum> AddClient(ClientDTO client);
        public Task<CommonEnum> EditClient(int id, ClientDTO editedClient);
        public Task<CommonEnum> DeleteClientById(int id);
    }
}
