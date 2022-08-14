using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Services.Interfaces;
using API.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implementations
{
    public class ServiceProviderDbRepository : IServiceProviderDbRepository
    {
        private readonly FWD_CompContext _context;

        public ServiceProviderDbRepository(FWD_CompContext context)
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
        public async Task<string> AddProvider(ServiceProviderDTO provider)
        {
            bool flag = false;
            string message = "";

            //check if exists
            string tax = await _context.ServiceProviders.Where(x => x.Tax == provider.Tax).Select(x => x.Tax).SingleOrDefaultAsync();
            if (tax == provider.Tax)
            {
                flag = false;
                message = "provider with given tax already exists";
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

                await _context.SaveChangesAsync();
                message = "provider successfully added";
            }
            return message;
        }
        public async Task<string> EditProvider(int id, ServiceProviderDTO editedProvider)
        {
            bool flag = false;
            string message = "";

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
                    message = "could not edit provider";
                    flag = false;
                }
            }
            else
            {
                flag = false;
                message = "contractor with given tax already exists";
            }

            if (flag)
            {
                await _context.SaveChangesAsync();
                message = "provider successfully edited";
            }

            return message;
        }
        public async Task<string> DeleteProviderById(int id)
        {
            bool flag = false;
            string message = "";

            //check if exists
            var providerFromDB = await _context.ServiceProviders.Where(x => x.Id.Equals(id)).SingleOrDefaultAsync();
            if (providerFromDB == null)
            {
                flag = false;
                message = "cannot find provider";
            }
            else
            {
                flag = true;
                _context.Remove(providerFromDB);
            }
            if (flag)
            {
                await _context.SaveChangesAsync();
                message = "provider removed";
            }

            return message;
        }
    }
}
