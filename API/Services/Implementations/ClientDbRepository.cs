using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Services.Interfaces;
using API.DTO;
using Microsoft.EntityFrameworkCore;
using API.Enum;

namespace API.Services.Implementations
{
    public class ClientDbRepository : IClientDbRepository
    {
        private readonly FWD_CompContext _context;

        public ClientDbRepository(FWD_CompContext context)
        {
            _context = context;
        }
        public async Task<ClientDTO> GetClientById(int id)
        {
            var client = await _context.Clients.Where(x => x.Id == id)
                .Select(m => new ClientDTO
                {
                    Name = m.Name,
                    Coutry = m.Coutry,
                    Zip = m.Zip,
                    Street = m.Street,
                    Tax = m.Tax,
                    Credit = m.Credit
                })
            .SingleOrDefaultAsync();

            return client;
        }
        public async Task<ICollection<ClientDTO>> GetClients()
        {
            var clients = await _context.Clients
                .Select(m => new ClientDTO
                {
                    Name = m.Name,
                    Coutry = m.Coutry,
                    Zip = m.Zip,
                    Street = m.Street,
                    Tax = m.Tax,
                    Credit = m.Credit
                })
                .ToListAsync();

            return clients;
        }
        public async Task<CommonEnum> AddClient(ClientDTO client)
        {
            bool flag = false;
            CommonEnum message = CommonEnum.UNKNOWN_ERROR;

            //check if exists
            string tax = await _context.Clients.Where(x => x.Tax == client.Tax).Select(x => x.Tax).SingleOrDefaultAsync();
            if (tax == client.Tax)
            {
                flag = false;
                message = CommonEnum.ALREADY_EXISTS;
                    //"contractor with given tax already exists";
            }
            else
                flag = true;

            if (flag)
            {
                await _context.AddAsync(new Client
                {
                   Name = client.Name,
                   Tax = client.Tax,
                   Street = client.Street,
                   Zip = client.Zip,
                   Coutry = client.Coutry,
                   Credit = client.Credit
                }
                );

                flag = await _context.SaveChangesAsync() > 0;
                message = CommonEnum.SUCCESSFULLY_ADDED;
                //"contractor successfully added";

                if (!flag)
                    message = CommonEnum.CANNOT_SAVE;
                        //"could not save changes";
            }
            return message;
        }
        public async Task<CommonEnum> EditClient(int id, ClientDTO editedClient)
        {
            bool flag = false;
            CommonEnum message = CommonEnum.UNKNOWN_ERROR;

            var contactorFromDB = await _context.Clients.Where(x => x.Id.Equals(id)).SingleOrDefaultAsync();
            //duplicate check
            var duplicate = await _context.Clients.Where(x => x.Tax == editedClient.Tax && x.Id != id).SingleOrDefaultAsync();

            if (duplicate == null)
            {
                //check if exists
                if (contactorFromDB != null)
                {
                    flag = true;
                    contactorFromDB.Name = editedClient.Name;
                    contactorFromDB.Tax = editedClient.Tax;
                    contactorFromDB.Zip = editedClient.Zip;
                    contactorFromDB.Street = editedClient.Street;
                    contactorFromDB.Coutry = editedClient.Coutry;
                    contactorFromDB.Credit = editedClient.Credit;
                }
                else
                {
                    message = CommonEnum.CANNOT_EDIT;
                        //"could not edit contractor";
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
                message = CommonEnum.CHANGES_SAVED;
                //"contractor successfully edited";
                if (!flag)
                    message = CommonEnum.CANNOT_SAVE;
                        //"could not save changes";
            }

            return message;
        }
        public async Task<CommonEnum> DeleteClientById(int id)
        {
            bool flag = false;
            CommonEnum message = CommonEnum.UNKNOWN_ERROR;

            //check if exists
            var contactorFromDB = await _context.Clients.Where(x => x.Id.Equals(id)).SingleOrDefaultAsync();
            if (contactorFromDB == null)
            {
                flag = false;
                message = CommonEnum.CANNOT_FIND;
                    //"cannot find contractor";
            }
            else
            {
                flag = true;
                _context.Remove(contactorFromDB);
            }
            if (flag) 
            {
                flag = await _context.SaveChangesAsync() > 0;
                message = CommonEnum.SUCCESSFULLY_REMOVED;
                //"contractor removed";
                if (!flag)
                    message = CommonEnum.CANNOT_SAVE;
                        //"could not svae changes";
            }

            return message;
        }
    }
}
