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

namespace Application.Repository.Implementations
{
    public class Login : ILoginService
    {
        private readonly FormupContext _context;
        private readonly ITokenService _tokenService;

        public Login(FormupContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        public async Task<UserResponseDTO> AddForwarder(ForwarderRequestDTO forwarder)
        {
            bool flag = false;
            UserResponseDTO response = null;
            Forwarder newForwarder = null;

            //check if exists
            Forwarder fwd = await _context.Forwarders
                .Where(x => x.Name == forwarder.Login)
                .SingleOrDefaultAsync();

            if (fwd != null)
            {
                if (fwd.Name.ToLower().Equals(forwarder.Login.ToLower()))
                {
                    response.ErrorMessage = "TODO";
                    //login taken
                }
                if (fwd.Surname.ToLower().Equals(forwarder.Surname.ToLower()) && fwd.Prefix.ToLower().Equals(forwarder.Prefix.ToLower()))
                {
                    response.ErrorMessage = "TODO";
                    //forwarder with given parameters already exists (surname + prefix)
                }
            }
            else
                flag = true;
            if (forwarder.PassHash.Length < 6)
            {
                response.ErrorMessage = "TODO";
                flag = false;
            }
            if (forwarder.Prefix.Length > 3)
            {
                response.ErrorMessage = "TODO";
                flag = false;
            }

            if (flag)
            {
                using var hmac = new HMACSHA512();

                newForwarder = new Forwarder
                {
                    Name = forwarder.Login,
                    Surname = forwarder.Surname,
                    Prefix = forwarder.Prefix.ToUpper(),
                    PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(forwarder.PassHash)),
                    PassSalt = hmac.Key
                };

                await _context.AddAsync(newForwarder);

                flag = await _context.SaveChangesAsync() > 0;
                response.ErrorMessage = "TODO";
                //forwarder successfully added

                if (!flag)
                {
                    response.ErrorMessage = "TODO";
                    //could not save changes
                }

                hmac.Dispose();
                response.UserName = newForwarder.Name;
                response.Token = _tokenService.CreateToken(newForwarder);
                response.ErrorMessage = "";

                return response;
            }

            return response;
        }
        public async Task<UserResponseDTO> UserLoginStatus(UserLoginDTO userDTO)
        {
            var userFromDb = await _context.Forwarders
                .SingleOrDefaultAsync(x => x.Name == userDTO.Login);
            UserResponseDTO userWithAccess = null;

            if (userFromDb == null)
            {
                return new UserResponseDTO
                {
                    UserName = string.Empty,
                    Token = string.Empty,
                    ErrorMessage = string.Empty
                };
            }
            else
            {
                using var hmac = new HMACSHA512(userFromDb.PassSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != userFromDb.PassHash[i])
                        userWithAccess.ErrorMessage = "invalid credentials";
                }
                userWithAccess = new
                {
                    UserName = userFromDb.Name,
                    Token = _tokenService.CreateToken(userFromDb),
                    ErrorMessage = string.Empty
                };

                hmac.Dispose();
            }

            return userWithAccess;
        }
    }
}
