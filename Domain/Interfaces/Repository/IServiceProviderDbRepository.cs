using Domain.DTO;
using Infrastructure.Models;

namespace Domain.Interfaces.Repository
{
    public interface IServiceProviderDbRepository
    {
        Task<ICollection<ServiceProviderDTO>> GetProviders();
        Task<ServiceProvider> GetProviderById(int id);
        Task<ServiceProvider> GetDuplicatedProvider(int id, string tax);
        Task<ServiceProviderDTO> AddProvider(ServiceProviderDTO providerDTO);
        Task<ServiceProviderDTO> EditProvider(ServiceProviderDTO editedProvider);
        Task<bool> DeleteProvider(ServiceProvider providerToDelete);
    }
}
