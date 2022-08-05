﻿using System;
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

        public async Task<ICollection<ClientDTO>> GetClients()
        {
            var montazFinansowy = await _context.Clients
                .Select(m => new ClientDTO
                {
                    Name=m.Name,
                    Coutry=m.Coutry,
                    Zip=m.Zip,
                    Street=m.Street,
                    Tax=m.Tax,
                    Credit=m.Credit
                })
                .ToListAsync();

            return montazFinansowy;
        }
    }
}
