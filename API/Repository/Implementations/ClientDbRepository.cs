using Domain.DTO;
using Domain.Helpers;
using Domain.Interfaces.Repository;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Repository.Implementations
{
    public class ClientDbRepository : IClientDbRepository
    {
        private readonly FormupContext _context;
        private readonly ILogger _logger;

        public ClientDbRepository(FormupContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ClientDTO> GetClientById(int id)
        {
            var response = await _context.Clients
                .Where(x => x.Id == id)
                .Select(m => new ClientDTO
                {
                    Name = m.Name,
                    Coutry = m.Coutry,
                    Zip = m.Zip,
                    Street = m.Street,
                    Tax = m.Tax,
                    Credit = m.Credit,
                    ErrorMessage = ""
                })
                .SingleOrDefaultAsync();


            return response;
        }

        public async Task<ICollection<ClientDTO>> GetClients()
        {

            var response = await _context.Clients
                .Select(m => new ClientDTO
                {
                    Name = m.Name,
                    Coutry = m.Coutry,
                    Zip = m.Zip,
                    Street = m.Street,
                    Tax = m.Tax,
                    Credit = m.Credit,
                    ErrorMessage = ""
                })
                .ToListAsync();


            return response;
        }

        public async Task<ClientDTO> AddClient(ClientDTO client)
        {
            ClientDTO response = (ClientDTO)ObjectCreationHelper.GenerateObject(typeof(ClientDTO));

            string tax = await _context.Clients
                    .Where(x => x.Tax == client.Tax)
                    .Select(x => x.Tax)
                    .SingleOrDefaultAsync();

            if (tax == client.Tax)
            {
                response.ErrorMessage = "this tax already exists";
                _logger.Warning($"Tried to add existing tax: {tax}");
                return response;
            }

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
                response = client;
            }


            return response;
        }
        public async Task<ClientDTO> EditClient(ClientDTO editedClient)
        {
            bool flag = false;
            CommonEnum message = CommonEnum.UNKNOWN_ERROR;

            var contactorFromDB = await _context.Clients.Where(x => x.Id.Equals(editedClient.Id)).SingleOrDefaultAsync();
            //duplicate check
            var duplicate = await _context.Clients.Where(x => x.Tax == editedClient.Tax && x.Id != editedClient.Id).SingleOrDefaultAsync();

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

            return response;
        }
        public async Task<ClientDTO> DeleteClientById(int id)
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

            return response;
        }
    }
}
