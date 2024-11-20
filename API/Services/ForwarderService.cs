﻿using Domain.CustomExceptions;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
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

        public async Task<ForwarderResponseDTO> EditForwarder(ForwarderRequestDTO forwarderToEdit)
        {
            var forwarderFromDb = await _forwarderDbRepository.GetForwarderById(forwarderToEdit.Id);
            var duplicate = await _forwarderDbRepository.GetDuplicatedForwarder(forwarderToEdit);
            if (duplicate == null)
            {
                if (forwarderFromDb != null)
                {
                    return await _forwarderDbRepository.EditForwarder(forwarderToEdit, forwarderFromDb);
                }
                throw new GetEntityException();
            }
            throw new GetEntityException($"Forwarder already exists {duplicate.Prefix}");
        }

        public async Task<ForwarderResponseDTO> GetForwarderById(int id)
        {
            var forwarderFromDb = await _forwarderDbRepository.GetForwarderById(id);

            return new ForwarderResponseDTO
            {
                Login = forwarderFromDb.Name,
                Prefix = forwarderFromDb.Prefix,
                Surname = forwarderFromDb.Surname,
                Id = forwarderFromDb.Id,
            };
        }

        public async Task<ICollection<ForwarderResponseDTO>> GetForwarders()
        {
            return await _forwarderDbRepository.GetForwarders();
        }
    }
}