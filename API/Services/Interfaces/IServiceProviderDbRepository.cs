using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;

namespace API.Services.Interfaces
{
    public interface IServiceProviderDbRepository
    {
        public Task<ICollection<ServiceProviderDTO>> GetProviders();
        public Task<ServiceProviderDTO> GetProviderById(int id);
        public Task<string> AddProvider(ServiceProviderDTO providerDTO);
        public Task<string> EditProvider(int id, ServiceProviderDTO editedProvider);
        public Task<string> DeleteProviderById(int id);
    }
}
