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
        public async Task<string> AddClient(ClientDTO client)
        {
            bool flag = false;
            string message = "";

            //check if exists
            string tax = await _context.Clients.Where(x => x.Tax == client.Tax).Select(x => x.Tax).SingleOrDefaultAsync();
            if (tax == client.Tax)
            {
                flag = false;
                message = "contractor with given tax already exists";
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

                await _context.SaveChangesAsync();
                message = "contractor successfully added";
            }
            return message;
        }
        public async Task<string> EditClient(int id, ClientDTO editedClient)
        {
            bool flag = false;
            string message = "";

            var contactorFromDB = await _context.Clients.Where(x => x.Id.Equals(id)).SingleOrDefaultAsync();

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
                message = "could not edit contractor";
                flag = false;
            }

            if (flag)
            {
                await _context.SaveChangesAsync();
                message = "contractor successfully edited";
            }

            return message;
        }
        public async Task<string> DeleteClientById(int id)
        {
            bool flag = false;
            string message = "";

            //check if exists
            var contactorFromDB = await _context.Clients.Where(x => x.Id.Equals(id)).SingleOrDefaultAsync();
            if (contactorFromDB == null)
            {
                flag = false;
                message = "cannot find contractor";
            }
            else
            {
                flag = true;
                _context.Remove(contactorFromDB);
            }
            if (flag) 
            {
                await _context.SaveChangesAsync();
                message = "contractor removed";
            }

            return message;
        }
    }
}
