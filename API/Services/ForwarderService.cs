using Domain.CustomExceptions;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Domain.StaticMappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ForwarderService : IForwarderService
    {
        private readonly IForwarderDbRepository _forwarderDbRepository;

        public ForwarderService(IForwarderDbRepository forwarderDbRepository)
        {
            _forwarderDbRepository = forwarderDbRepository;
        }

        public async Task<bool> DeleteForwarderById(int id)
        {
            var forwarderToDelete = await _forwarderDbRepository.GetForwarderById(id);
            return forwarderToDelete == null ? throw new GetEntityException() : await _forwarderDbRepository.DeleteForwarder(forwarderToDelete);
        }

        public async Task<bool> EditForwarder(ForwarderRequestDTO forwarderToEdit)
        {
            var forwarderFromDb = await _forwarderDbRepository.GetForwarderById(forwarderToEdit.Id);
            var duplicate = await _forwarderDbRepository.GetDuplicatedForwarder(forwarderToEdit);

            if (duplicate != null)
            {
                throw new SavingException($"Forwarder already exists {duplicate.Prefix}");
            }
            if (forwarderFromDb == null)
            {
                throw new GetEntityException();
            }

            return await _forwarderDbRepository.EditForwarder(forwarderToEdit, forwarderFromDb);
        }

        public async Task<ForwarderResponseDTO> GetForwarderById(int id)
        {
            var forwarderFromDb = await _forwarderDbRepository.GetForwarderById(id);
            return ForwarderMapper.MapForwarderToForwarderResponseDTO(forwarderFromDb);
        }

        public async Task<ICollection<ForwarderResponseDTO>> GetForwarders()
        {
            return await _forwarderDbRepository.GetForwarders();
        }
    }
}
