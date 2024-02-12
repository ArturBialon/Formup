using Domain.CustomExceptions;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.Repository;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository.Implementations
{
    public class ForwarderDbRepository : IForwarderDbRepository
    {
        private readonly FormupContext _context;

        public ForwarderDbRepository(FormupContext context)
        {
            _context = context;
        }

        public async Task<Forwarder> GetForwarderById(int id)
        {
            var forwarder = await _context.Forwarders.Where(x => x.Id == id)
            .SingleOrDefaultAsync();
            return forwarder ?? throw new GetEntityException();
        }

        public async Task<Forwarder> GetDuplicatedForwarder(ForwarderRequestDTO forwarder)
        {
            var duplicate = await _context.Forwarders
                .Where(
                x => x.Name.ToLower() == forwarder.Login.ToLower()
                && x.Id != forwarder.Id
                || x.Surname.ToLower() == forwarder.Surname.ToLower()
                && x.Id != forwarder.Id
                || x.Prefix.ToLower() == forwarder.Prefix.ToLower()
                && x.Id != forwarder.Id)
                .SingleOrDefaultAsync();

            return duplicate;
        }

        public async Task<ICollection<ForwarderResponseDTO>> GetForwarders()
        {
            var forwarders = await _context.Forwarders
                .Select(m => new ForwarderResponseDTO
                {
                    Login = m.Name,
                    Surname = m.Surname,
                    Prefix = m.Prefix
                })
            .ToListAsync();

            return forwarders;
        }

        public async Task<bool> DeleteForwarder(Forwarder forwarderToDelete)
        {
            _context.Forwarders.Remove(forwarderToDelete);

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException();
        }

        public async Task<ForwarderResponseDTO> EditForwarder(ForwarderRequestDTO editedForwarder, Forwarder forwarderFromDB)
        {
            using var hmac = new HMACSHA512();

            forwarderFromDB.Name = editedForwarder.Login;
            forwarderFromDB.Surname = editedForwarder.Surname;
            forwarderFromDB.Prefix = editedForwarder.Prefix.ToUpper();
            forwarderFromDB.PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(editedForwarder.PassHash));
            forwarderFromDB.PassSalt = hmac.Key;

            if (await _context.SaveChangesAsync() > 0)
            {
                return new ForwarderResponseDTO
                {
                    Login = forwarderFromDB.Name,
                    Surname = forwarderFromDB.Surname,
                    Prefix = forwarderFromDB.Prefix,
                    Id = forwarderFromDB.Id
                };
            }

            throw new SavingException();
        }
    }
}
