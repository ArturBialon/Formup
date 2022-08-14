using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interfaces;
using API.Models;
using API.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implementations
{
    public class ForwardersDbRepository : IForwardersDbRepository
    {
        private readonly FWD_CompContext _context;

        public ForwardersDbRepository(FWD_CompContext context)
        {
            _context = context;
        }

        public async Task<string> AddForwarder(ForwarderDTO forwarder)
        {
            bool flag = false;
            string message = "";

            //check if exists
            Forwarder fwd = await _context.Forwarders
                .Where(x => x.Name == forwarder.Name && x.Surname == forwarder.Surname && x.Prefix == forwarder.Prefix)
                .SingleOrDefaultAsync();

            if (fwd != null)
            {
                if (fwd.Name == forwarder.Name && fwd.Surname == forwarder.Surname && fwd.Prefix == forwarder.Prefix)
                {
                    flag = false;
                    message = "forwarder with given parameters already exists";
                }
            }
            else
                flag = true;

            if (flag)
            {
                await _context.AddAsync(new Forwarder
                {
                    Name = forwarder.Name,
                    Surname = forwarder.Surname,
                    Prefix = forwarder.Prefix
                }
                );

                await _context.SaveChangesAsync();
                message = "forwarder successfully added";
            }
            return message;
        }

        public async Task<string> DeleteForwarderById(int id)
        {
            bool flag = false;
            string message = "";

            //check if exists
            var forwarderFromDB = await _context.Forwarders.Where(x => x.Id.Equals(id)).SingleOrDefaultAsync();
            if (forwarderFromDB == null)
            {
                flag = false;
                message = "cannot find forwarder";
            }
            else
            {
                flag = true;
                _context.Remove(forwarderFromDB);
            }
            if (flag)
            {
                await _context.SaveChangesAsync();
                message = "forwarder removed";
            }

            return message;
        }

        public async Task<string> EditForwarder(int id, ForwarderDTO editedForwarder)
        {
            bool flag = false;
            string message = "";

            var forwarderFromDB = await _context.Forwarders.Where(x => x.Id.Equals(id)).SingleOrDefaultAsync();

            //check if exists
            if (forwarderFromDB != null)
            {
                flag = true;
                forwarderFromDB.Name = editedForwarder.Name;
                forwarderFromDB.Surname = editedForwarder.Surname;
                forwarderFromDB.Prefix = editedForwarder.Prefix;
            }
            else
            {
                message = "could not edit forwarder";
                flag = false;
            }

            if (flag)
            {
                await _context.SaveChangesAsync();
                message = "forwarder successfully edited";
            }

            return message;
        }

        public async Task<ForwarderDTO> GetForwarderById(int id)
        {
            var forwarder = await _context.Forwarders.Where(x => x.Id == id)
                .Select(m => new ForwarderDTO
                {
                    Name = m.Name,
                    Surname = m.Surname,
                    Prefix = m.Prefix
                })
            .SingleOrDefaultAsync();

            return forwarder;
        }

        public async Task<ICollection<ForwarderDTO>> GetForwarders()
        {
            var forwarders = await _context.Forwarders
                .Select(m => new ForwarderDTO
                {
                    Name = m.Name,
                    Surname = m.Surname,
                    Prefix = m.Prefix
                })
            .ToListAsync();

            return forwarders;
        }
    }
}
