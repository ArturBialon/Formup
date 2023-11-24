using API.DTO;
using API.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface IServiceProviderDbRepository
    {
        public Task<ICollection<ServiceProviderDTO>> GetProviders();
        public Task<ServiceProviderDTO> GetProviderById(int id);
        public Task<CommonEnum> AddProvider(ServiceProviderDTO providerDTO);
        public Task<CommonEnum> EditProvider(int id, ServiceProviderDTO editedProvider);
        public Task<CommonEnum> DeleteProviderById(int id);
    }
}
