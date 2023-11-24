using API.DTO;
using API.DTO.Request;
using API.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Services.Implementations
{
    public class ForwardersDbRepository : IForwardersDbRepository
    {
        private readonly FWD_CompContext _context;

        public ForwardersDbRepository(FWD_CompContext context)
        {
            _context = context;
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
                flag = await _context.SaveChangesAsync() > 0;
                message = "forwarder removed";
                if (!flag)
                    message += "could not save changes";
            }

            return message;
        }

        public async Task<string> EditForwarder(int id, ForwarderAddDTO editedForwarder)
        {
            bool flag = false;
            string message = "";

            var forwarderFromDB = await _context.Forwarders.Where(x => x.Id.Equals(id)).SingleOrDefaultAsync();
            using var hmac = new HMACSHA512();
            //duplicate chceck
            var duplicate = await _context.Forwarders
                .Where(
                x => x.Name.ToLower() == editedForwarder.Login.ToLower()
                && x.Id != id
                || x.Surname.ToLower() == editedForwarder.Surname.ToLower()
                && x.Prefix.ToLower() == editedForwarder.Prefix.ToLower()
                && x.Id != id)
                .SingleOrDefaultAsync();
            if (duplicate == null)
            {
                //check if exists
                if (forwarderFromDB != null)
                {
                    flag = true;
                    forwarderFromDB.Name = editedForwarder.Login;
                    forwarderFromDB.Surname = editedForwarder.Surname;
                    forwarderFromDB.Prefix = editedForwarder.Prefix.ToUpper();
                    forwarderFromDB.PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(editedForwarder.PassHash));
                    forwarderFromDB.PassSalt = hmac.Key;
                }
                else
                {
                    message = "could not edit forwarder that does not exists";
                    flag = false;
                }
            }
            else
            {
                flag = false;
                message = "forwarder with given parameters already exists";
            }

            if (flag)
            {
                flag = await _context.SaveChangesAsync() > 0;
                message = "forwarder successfully edited";
                if (!flag)
                    message += "could not save changes";
            }

            return message;
        }

        public async Task<ForwarderDTO> GetForwarderById(int id)
        {
            var forwarder = await _context.Forwarders.Where(x => x.Id == id)
                .Select(m => new ForwarderDTO
                {
                    Login = m.Name,
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
                    Login = m.Name,
                    Surname = m.Surname,
                    Prefix = m.Prefix
                })
            .ToListAsync();

            return forwarders;
        }
    }
}
