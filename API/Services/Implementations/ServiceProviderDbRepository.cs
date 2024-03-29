﻿using Application.Services.Interfaces;
using Domain.DTO;
using Domain.Enum;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.Implementations
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
        public async Task<CommonEnum> AddProvider(ServiceProviderDTO provider)
        {
            bool flag = false;
            CommonEnum message = CommonEnum.UNKNOWN_ERROR;

            //check if exists
            string tax = await _context.ServiceProviders.Where(x => x.Tax == provider.Tax).Select(x => x.Tax).SingleOrDefaultAsync();
            if (tax == provider.Tax)
            {
                flag = false;
                message = CommonEnum.ALREADY_EXISTS;
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
                message = CommonEnum.SUCCESSFULLY_ADDED;
                //"provider successfully added";
                if (!flag)
                    message = CommonEnum.CANNOT_SAVE;
                //" could not save changes";
            }
            return message;
        }
        public async Task<CommonEnum> EditProvider(int id, ServiceProviderDTO editedProvider)
        {
            bool flag = false;
            CommonEnum message = CommonEnum.UNKNOWN_ERROR;

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
                    message = CommonEnum.CANNOT_FIND;
                    //"provider with given id does not exists";
                    flag = false;
                }
            }
            else
            {
                flag = false;
                message = CommonEnum.ALREADY_EXISTS;
                //"contractor with given tax already exists";
            }
            if (flag)
            {
                flag = await _context.SaveChangesAsync() > 0;
                if (flag)
                    message = CommonEnum.CHANGES_SAVED;
                //"provider successfully edited";
                else
                    message = CommonEnum.CANNOT_SAVE;
                //" could not save changes";
            }

            return message;
        }
        public async Task<CommonEnum> DeleteProviderById(int id)
        {
            bool flag = false;
            CommonEnum message = CommonEnum.UNKNOWN_ERROR;

            //check if exists
            var providerFromDB = await _context.ServiceProviders.Where(x => x.Id.Equals(id)).SingleOrDefaultAsync();
            if (providerFromDB == null)
            {
                flag = false;
                message = CommonEnum.CANNOT_FIND;
                //"cannot find provider";
            }
            else
            {
                _context.Remove(providerFromDB);
                flag = await _context.SaveChangesAsync() > 0;
            }

            if (flag)
            {
                message = CommonEnum.SUCCESSFULLY_REMOVED;
                //"provider removed";
            }
            else
            {
                message = CommonEnum.CANNOT_SAVE;
                //"could not save changes";
            }

            return message;
        }
    }
}
