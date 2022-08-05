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
    }
}
