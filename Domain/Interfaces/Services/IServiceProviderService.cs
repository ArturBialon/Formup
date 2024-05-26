using Domain.DTO;

namespace Domain.Interfaces.Services
{
    public interface IServiceProviderService
    {
        public Task<ICollection<ServiceProviderDTO>> GetProviders();
        public Task<ServiceProviderDTO> GetProviderById(int id);
        public Task<ServiceProviderDTO> AddProvider(ServiceProviderDTO providerDTO);
        public Task<ServiceProviderDTO> EditProvider(ServiceProviderDTO editedProvider);
        public Task<bool> DeleteProvider(int id);
    }
}
