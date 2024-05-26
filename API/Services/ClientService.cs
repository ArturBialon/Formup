using Domain.CustomExceptions;
using Domain.DTO;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientDbRepository _clientDbRepository;
        private readonly ILogger _logger;

        public ClientService(IClientDbRepository clientDbRepository)
        {
            _clientDbRepository = clientDbRepository;
        }

        public async Task<ICollection<ClientDTO>> GetClients()
        {
            return await _clientDbRepository.GetClients();
        }

        public async Task<ClientDTO> GetClientById(int id)
        {
            var client = await _clientDbRepository.GetClientById(id);

            return new ClientDTO
            {
                Id = id,
                Name = client.Name,
                Tax = client.Tax,
                Street = client.Street,
                Zip = client.Zip,
                Coutry = client.Coutry,
                Credit = client.Credit
            };
        }

        public async Task<ClientDTO> AddClient(ClientDTO client)
        {
            var contractorFromDb = await _clientDbRepository.GetClientByTax(client.Tax);

            if (contractorFromDb.Tax == client.Tax)
            {
                _logger.Information($"Tried to add existing tax: {contractorFromDb.Tax}, {contractorFromDb.Name}");
                throw new GetEntityException($"Tax is already in database:  {contractorFromDb.Tax}, {contractorFromDb.Name}");
            }

            return await _clientDbRepository.AddClient(client);
        }

        public async Task<ClientDTO> EditClient(ClientDTO editedClient)
        {
            var contractorFromDb = await _clientDbRepository.GetClientById(editedClient.Id);
            var duplicate = await _clientDbRepository.GetDuplicatedClient(editedClient.Tax, editedClient.Id);

            if (duplicate == null)
            {
                if (contractorFromDb != null)
                {
                    return await _clientDbRepository.EditClient(editedClient, contractorFromDb);
                }
                else
                {
                    throw new GetEntityException();
                }
            }
            else
            {
                _logger.Information($"Tax is already in database:  {duplicate.Tax}, {duplicate.Name}");
                throw new SavingException("Client with this tax already exists");
            }

            throw new SavingException();
        }

        public async Task<bool> DeleteClientById(int id)
        {
            var clientToDelete = await _clientDbRepository.GetClientById(id);
            return await _clientDbRepository.DeleteClient(clientToDelete);
        }
    }
}
