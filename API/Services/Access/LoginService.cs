using Domain.CustomExceptions;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.UserAccessService;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly FormupContext _context;
        private readonly ITokenService _tokenService;

        public LoginService(FormupContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        public async Task<UserResponseDTO> AddForwarder(ForwarderRequestDTO forwarder)
        {
            UserResponseDTO response = null;
            Forwarder newForwarder = null;

            Forwarder fwd = await _context.Forwarders
                .Where(x => x.Name == forwarder.Login)
                .SingleOrDefaultAsync();

            if (fwd != null)
            {
                if (fwd.Name.ToLower().Equals(forwarder.Login.ToLower()) || fwd.Prefix.ToLower().Equals(forwarder.Prefix.ToLower()))
                {
                    throw new RegistrationException("Login or prefix taken");
                }
            }
            if (forwarder.PassHash.Length < 6)
            {
                throw new RegistrationException("Password is to short, must be minimum 6 characters long");
            }
            if (forwarder.Prefix.Length > 4)
            {
                throw new RegistrationException("Prefix must be maximum 4 characters long");
            }

            using var hmac = new HMACSHA512();

            newForwarder = new Forwarder
            {
                Name = forwarder.Login,
                Surname = forwarder.Surname,
                Prefix = forwarder.Prefix.ToUpper(),
                PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(forwarder.PassHash)),
                PassSalt = hmac.Key
            };

            _context.Add(newForwarder);

            if (await _context.SaveChangesAsync() > 0)
            {
                response.UserName = newForwarder.Name;
                response.Token.AccessToken = _tokenService.CreateToken(newForwarder);
                hmac.Dispose();

                return response;
            }

            throw new SavingException();
        }


        public async Task<UserResponseDTO> UserLoginStatus(UserLoginDTO userDTO)
        {
            var userFromDb = await _context.Forwarders
                .SingleOrDefaultAsync(x => x.Name == userDTO.Login);

            if (userFromDb == null)
            {
                throw new LoginException();
            }
            else
            {
                using var hmac = new HMACSHA512(userFromDb.PassSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != userFromDb.PassHash[i])
                        throw new LoginException();
                }

                hmac.Dispose();

                return new UserResponseDTO
                {
                    UserName = userFromDb.Name,
                    Token = new Domain.DTO.ResponseTokenDTO { AccessToken = _tokenService.CreateToken(userFromDb) }
                };
            }
        }
    }
}
