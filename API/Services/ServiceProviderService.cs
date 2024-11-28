using Domain.CustomExceptions;
using Domain.DTO;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Domain.StaticMappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ServiceProviderService : IServiceProviderService
    {
        private readonly IServiceProviderDbRepository _serviceProviderRepository;

        public ServiceProviderService(IServiceProviderDbRepository serviceProviderRepository)
        {
            _serviceProviderRepository = serviceProviderRepository;
        }

        public async Task<ServiceProviderDTO> GetProviderById(int id)
        {
            var provider = await _serviceProviderRepository.GetProviderById(id);
            return ServiceProviderMapper.MapServiceProviderToDto(provider);
        }

        public async Task<ICollection<ServiceProviderDTO>> GetProviders()
        {
            var providers = await _serviceProviderRepository.GetProviders();
            return providers;
        }

        public async Task<bool> AddProvider(ServiceProviderDTO provider)
        {
            var isAdded = await _serviceProviderRepository.AddProvider(provider);
            return isAdded;
        }

        public async Task<bool> EditProvider(ServiceProviderDTO editedProvider)
        {
            var duplicatedProvider = await _serviceProviderRepository.GetDuplicatedProvider(editedProvider.Id, editedProvider.Tax);
            var providerFromDb = await _serviceProviderRepository.GetProviderById(editedProvider.Id);

            if (duplicatedProvider != null)
            {
                throw new SavingException("Provided tax already exists in database");
            }

            var isEdited = await _serviceProviderRepository.EditProvider(editedProvider, providerFromDb);
            return isEdited;
        }

        public async Task<bool> DeleteProvider(int id)
        {
            var providerFromDB = await _serviceProviderRepository.GetProviderById(id);
            return providerFromDB == null ? throw new GetEntityException() : await _serviceProviderRepository.DeleteProvider(providerFromDB);
        }
    }
}
