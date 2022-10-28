using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interfaces;
using API.Models;
using API.DTO;
using API.DTO.Request;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Services.Implementations
{
    public class ForwardersDbRepository : IForwardersDbRepository
    {
        private readonly FWD_CompContext _context;

        public ForwardersDbRepository(FWD_CompContext context)
        {
            _context = context;
        }

        public async Task<string> AddForwarder(ForwarderAddDTO forwarder)
        {
            bool flag = false;
            string message = "";

            //check if exists
            Forwarder fwd = await _context.Forwarders
                .Where(x => x.Name == forwarder.Login)
                .SingleOrDefaultAsync();

            if (fwd != null)
            {
                if(fwd.Name.Equals(forwarder.Login))
                {
                    message = "login taken : ";
                }
                if (fwd.Name.Equals(forwarder.Login) && fwd.Surname.Equals(forwarder.Surname) && fwd.Prefix.Equals(forwarder.Prefix))
                {
                    flag = false;
                    message += "forwarder with given parameters already exists (surname + prefix)";
                }
            }
            else
                flag = true;

            if (flag)
            {
                using var hmac = new HMACSHA512();

                await _context.AddAsync(new Forwarder
                {
                    Name = forwarder.Login,
                    Surname = forwarder.Surname,
                    Prefix = forwarder.Prefix,
                    PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(forwarder.PassHash)),
                    PassSalt = hmac.Key
                }
                );

                flag = await _context.SaveChangesAsync() > 0;
                message = "forwarder successfully added";

                if (!flag) 
                { 
                    message += "could not save changes"; 
                }

                hmac.Dispose();
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
                flag = await _context.SaveChangesAsync() > 0;
                message = "forwarder removed";
                if (!flag)
                    message += "could not save changes";
            }

            return message;
        }

        public async Task<string> EditForwarder(int id, ForwarderDTO editedForwarder)
        {
            bool flag = false;
            string message = "";

            var forwarderFromDB = await _context.Forwarders.Where(x => x.Id.Equals(id)).SingleOrDefaultAsync();
            
            //duplicate chceck
            var duplicate = await _context.Forwarders
                .Where(x => x.Name == editedForwarder.Login && x.Surname == editedForwarder.Surname && x.Prefix == editedForwarder.Prefix && x.Id != id)
                .SingleOrDefaultAsync();
            if (duplicate == null)
            {
                //check if exists
                if (forwarderFromDB != null)
                {
                    flag = true;
                    forwarderFromDB.Name = editedForwarder.Login;
                    forwarderFromDB.Surname = editedForwarder.Surname;
                    forwarderFromDB.Prefix = editedForwarder.Prefix;
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
                message = "forwarde with given parameters already exists";
            }

            if (flag)
            {
                flag = await _context.SaveChangesAsync() >0;
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
