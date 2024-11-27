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
    public class ClientDbRepository : IClientDbRepository
    {
        private readonly FormupContext _context;

        public ClientDbRepository(FormupContext context)
        {
            _context = context;
        }

        public async Task<Client> GetClientById(int id)
        {
            var response = await _context.Clients
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();

            return response ?? throw new GetEntityException();
        }

        public async Task<Client> GetClientByTax(string tax)
        {
            var response = await _context.Clients
                .Where(x => x.Tax == tax)
                .SingleOrDefaultAsync();

            return response;
        }

        public async Task<Client> GetDuplicatedClient(string tax, int id)
        {
            var response = await _context.Clients
                .Where(x => x.Tax == tax && x.Id != id)
                .SingleOrDefaultAsync();

            return response;
        }

        public async Task<ICollection<ClientDTO>> GetClients()
        {
            var response = await _context.Clients
                .Select(client => ClientMapper.MapClientToClientDto(client))
                .ToListAsync();

            return response;
        }

        public async Task<bool> AddClient(ClientDTO client)
        {
            await _context.AddAsync(new Client
            {
                Name = client.Name,
                Tax = client.Tax,
                Street = client.Street,
                Zip = client.Zip,
                Coutry = client.Coutry,
                Credit = client.Credit
            });

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException();
        }

        public async Task<bool> EditClient(ClientDTO editedClient, Client contactorFromDB)
        {
            contactorFromDB.Name = editedClient.Name;
            contactorFromDB.Tax = editedClient.Tax;
            contactorFromDB.Zip = editedClient.Zip;
            contactorFromDB.Street = editedClient.Street;
            contactorFromDB.Coutry = editedClient.Coutry;
            contactorFromDB.Credit = editedClient.Credit;

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException();
        }

        public async Task<bool> DeleteClient(Client contactorFromDB)
        {
            _context.Remove(contactorFromDB);
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException($"Could not remove client {contactorFromDB.Tax}, {contactorFromDB.Name}");
        }
    }
}
