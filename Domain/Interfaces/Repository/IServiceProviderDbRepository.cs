using Domain.DTO;

namespace Domain.Interfaces.Repository
{
    public interface IServiceProviderDbRepository
    {
        public Task<ICollection<ServiceProviderDTO>> GetProviders();
        public Task<ServiceProviderDTO> GetProviderById(int id);
        public Task<ServiceProviderDTO> AddProvider(ServiceProviderDTO providerDTO);
        public Task<ServiceProviderDTO> EditProvider(int id, ServiceProviderDTO editedProvider);
        public Task<ServiceProviderDTO> DeleteProviderById(int id);
    }
}
