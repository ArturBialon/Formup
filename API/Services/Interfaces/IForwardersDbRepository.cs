using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.DTO.Request;

namespace API.Services.Interfaces
{
    public interface IForwardersDbRepository
    {
        public Task<ICollection<ForwarderDTO>> GetForwarders();
        public Task<ForwarderDTO> GetForwarderById(int id);
        public Task<string> EditForwarder(int id, ForwarderDTO editedClient);
        public Task<string> DeleteForwarderById(int id);
    }
}
