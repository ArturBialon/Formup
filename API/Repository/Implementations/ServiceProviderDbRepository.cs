using Domain.DTO;
using Domain.Interfaces.Repository;
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
        public async Task<ServiceProviderDTO> GetProviderById(int id)
        {
            var provider = await _context.ServiceProviders.Where(x => x.Id == id)
                .Select(m => new ServiceProviderDTO
                {
                    Name = m.Name,
                    Coutry = m.Coutry,
                    Zip = m.Zip,
                    Street = m.Street,
                    Tax = m.Tax
                })
            .SingleOrDefaultAsync();

            return provider;
        }
        public async Task<ICollection<ServiceProviderDTO>> GetProviders()
        {
            var providers = await _context.ServiceProviders
                .Select(m => new ServiceProviderDTO
                {
                    Name = m.Name,
                    Coutry = m.Coutry,
                    Zip = m.Zip,
                    Street = m.Street,
                    Tax = m.Tax
                })
                .ToListAsync();

            return providers;
        }
        public async Task<ServiceProviderDTO> AddProvider(ServiceProviderDTO provider)
        {
            bool flag = false;
            ServiceProviderDTO response = null;

            //check if exists
            string tax = await _context.ServiceProviders.Where(x => x.Tax == provider.Tax).Select(x => x.Tax).SingleOrDefaultAsync();
            if (tax == provider.Tax)
            {
                flag = false;
                response.ErrorMessage = "";
                //"provider with given tax already exists";
            }
            else
                flag = true;

            if (flag)
            {
                await _context.AddAsync(new ServiceProvider
                {
                    Name = provider.Name,
                    Tax = provider.Tax,
                    Street = provider.Street,
                    Zip = provider.Zip,
                    Coutry = provider.Coutry
                }
                );

                flag = await _context.SaveChangesAsync() > 0;
                response.ErrorMessage = "TODO";
                //"provider successfully added";
                if (!flag)
                    response.ErrorMessage = "TODO";
                //" could not save changes";
            }
            return response;
        }
        public async Task<ServiceProviderDTO> EditProvider(int id, ServiceProviderDTO editedProvider)
        {
            bool flag = false;
            ServiceProviderDTO response = null;

            var contactorFromDB = await _context.ServiceProviders.Where(x => x.Id.Equals(id)).SingleOrDefaultAsync();
            //check for duplicate
            var duplicate = await _context.ServiceProviders.Where(x => x.Tax == editedProvider.Tax && x.Id != id).SingleOrDefaultAsync();
            if (duplicate == null)
            {
                //check if exists
                if (contactorFromDB != null)
                {
                    flag = true;
                    contactorFromDB.Name = editedProvider.Name;
                    contactorFromDB.Tax = editedProvider.Tax;
                    contactorFromDB.Zip = editedProvider.Zip;
                    contactorFromDB.Street = editedProvider.Street;
                    contactorFromDB.Coutry = editedProvider.Coutry;
                }
                else
                {
                    flag = false;
                    response.ErrorMessage = "TODO";
                    //"provider with given id does not exists";
                }
            }
            else
            {
                flag = false;
                response.ErrorMessage = "TODO";
                //"contractor with given tax already exists";
            }
            if (flag)
            {
                flag = await _context.SaveChangesAsync() > 0;
                if (flag)
                    response.ErrorMessage = "TODO";
                //"provider successfully edited";
                else
                    response.ErrorMessage = "TODO";
                //" could not save changes";
            }

            return response;
        }
        public async Task<ServiceProviderDTO> DeleteProviderById(int id)
        {
            bool isDeleted = false;
            ServiceProviderDTO response = null;

            var providerFromDB = await _context.ServiceProviders.Where(x => x.Id.Equals(id)).SingleOrDefaultAsync();

            if (providerFromDB == null)
            {
                isDeleted = false;
                response.ErrorMessage = "Cannot find service provider";
            }
            else
            {
                _context.Remove(providerFromDB);
                isDeleted = await _context.SaveChangesAsync() > 0;
            }

            if (isDeleted)
            {
                response.ErrorMessage = "";
            }
            else
            {
                response.ErrorMessage = "Could not save changes";
            }

            return response;
        }
    }
}
