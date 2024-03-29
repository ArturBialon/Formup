﻿using Domain.DTO;
using Domain.DTO.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IForwardersDbRepository
    {
        public Task<ICollection<ForwarderDTO>> GetForwarders();
        public Task<ForwarderDTO> GetForwarderById(int id);
        public Task<string> EditForwarder(int id, ForwarderAddDTO editedClient);
        public Task<string> DeleteForwarderById(int id);
    }
}
