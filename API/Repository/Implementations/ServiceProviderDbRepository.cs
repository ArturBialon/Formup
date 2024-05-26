using Domain.CustomExceptions;
using Domain.DTO;
using Domain.Interfaces.Repository;
using Domain.StaticMappers;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Repository.Implementations
{
    public class ServiceProviderDbRepository : IServiceProviderDbRepository
    {
        private readonly FormupContext _context;

        public ServiceProviderDbRepository(FormupContext context)
        {
            _context = context;
        }

        public async Task<ServiceProvider> GetProviderById(int id)
        {
            var provider = await _context.ServiceProviders.Where(x => x.Id == id)
            .SingleOrDefaultAsync();

            return provider ?? throw new GetEntityException();
        }

        public async Task<ServiceProvider> GetDuplicatedProvider(int id, string tax)
        {
            var duplicate = await _context.ServiceProviders.Where(x => x.Tax == tax && x.Id != id).SingleOrDefaultAsync();
            return duplicate;
        }

        public async Task<ICollection<ServiceProviderDTO>> GetProviders()
        {
            var providers = await _context.ServiceProviders
                .Select(provider => ServiceProviderMapper.MapServiceProviderToDto(provider))
                .ToListAsync();

            return providers;
        }

        public async Task<ServiceProviderDTO> AddProvider(ServiceProviderDTO provider)
        {

            await _context.AddAsync(new ServiceProvider
            {
                Name = provider.Name,
                Tax = provider.Tax,
                Street = provider.Street,
                Zip = provider.Zip,
                Coutry = provider.Coutry
            });

            if (await _context.SaveChangesAsync() > 0) return provider;
            throw new SavingException();
        }

        public async Task<ServiceProviderDTO> EditProvider(ServiceProviderDTO editedProvider)
        {
            var contactorFromDB = await GetProviderById(editedProvider.Id);

            contactorFromDB.Name = editedProvider.Name;
            contactorFromDB.Tax = editedProvider.Tax;
            contactorFromDB.Zip = editedProvider.Zip;
            contactorFromDB.Street = editedProvider.Street;
            contactorFromDB.Coutry = editedProvider.Coutry;

            if (await _context.SaveChangesAsync() > 0) return editedProvider;
            throw new SavingException();
        }

        public async Task<bool> DeleteProvider(ServiceProvider providerToDelete)
        {
            _context.Remove(providerToDelete);

            if (await _context.SaveChangesAsync() > 0) return true;
            throw new SavingException("Could not save changes");
        }
    }
}
