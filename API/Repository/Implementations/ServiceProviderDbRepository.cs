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

        public async Task<bool> AddProvider(ServiceProviderDTO provider)
        {

            await _context.AddAsync(new ServiceProvider
            {
                Name = provider.Name,
                Tax = provider.Tax,
                Street = provider.Street,
                Zip = provider.Zip,
                Coutry = provider.Coutry
            });

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException();
        }

        public async Task<bool> EditProvider(ServiceProviderDTO editedProvider, ServiceProvider providerFromDb)
        {
            providerFromDb.Name = editedProvider.Name;
            providerFromDb.Tax = editedProvider.Tax;
            providerFromDb.Zip = editedProvider.Zip;
            providerFromDb.Street = editedProvider.Street;
            providerFromDb.Coutry = editedProvider.Coutry;

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException();
        }

        public async Task<bool> DeleteProvider(ServiceProvider providerToDelete)
        {
            _context.Remove(providerToDelete);

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException("Could not save changes");
        }
    }
}
