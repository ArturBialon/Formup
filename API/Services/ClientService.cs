using Domain.CustomExceptions;
using Domain.DTO;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Domain.StaticMappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientDbRepository _clientDbRepository;

        public ClientService(IClientDbRepository clientDbRepository)
        {
            _clientDbRepository = clientDbRepository;
        }

        public async Task<ICollection<ClientDTO>> GetClients()
        {
            return await _clientDbRepository.GetClients();
        }

        public async Task<ClientDTO> GetClientById(Guid id)
        {
            var client = await _clientDbRepository.GetClientById(id);

            return ClientMapper.MapClientToClientDto(client);
        }

        public async Task<bool> AddClient(ClientDTO client)
        {
            var contractorFromDb = await _clientDbRepository.GetClientByTax(client.Tax);

            if (contractorFromDb != null)
            {
                throw new GetEntityException($"Tax is already in database:  {contractorFromDb.Tax}, {contractorFromDb.Name}");
            }

            return await _clientDbRepository.AddClient(client);
        }

        public async Task<bool> EditClient(ClientDTO editedClient)
        {
            var contractorFromDb = await _clientDbRepository.GetClientById(editedClient.Id);
            var duplicate = await _clientDbRepository.GetDuplicatedClient(editedClient.Tax, editedClient.Id);

            if (duplicate != null)
            {
                throw new SavingException("Client with this tax already exists");
            }

            if (contractorFromDb == null)
            {
                throw new GetEntityException();
            }

            return await _clientDbRepository.EditClient(editedClient, contractorFromDb);
        }

        public async Task<bool> DeleteClientById(Guid id)
        {
            var clientToDelete = await _clientDbRepository.GetClientById(id);
            return await _clientDbRepository.DeleteClient(clientToDelete);
        }
    }
}
