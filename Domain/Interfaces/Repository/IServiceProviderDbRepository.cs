using Domain.DTO;
using Infrastructure.Models;

namespace Domain.Interfaces.Repository
{
    public interface IServiceProviderDbRepository
    {
        Task<ICollection<ServiceProviderDTO>> GetProviders();
        Task<ServiceProvider> GetProviderById(ServiceProvider.EntityId id);
        Task<ServiceProvider> GetDuplicatedProvider(ServiceProvider.EntityId id, string tax);
        Task<bool> AddProvider(ServiceProviderDTO providerDTO);
        Task<bool> EditProvider(ServiceProviderDTO editedProvider, ServiceProvider providerFromDb);
        Task<bool> DeleteProvider(ServiceProvider providerToDelete);
    }
}
