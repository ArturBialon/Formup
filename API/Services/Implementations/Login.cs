using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.Request;
using API.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using API.Enum;
using System.Security.Cryptography;
using System.Text;

namespace API.Services.Implementations
{
    public class Login : ILogin
    {
        private readonly FWD_CompContext _context;
        private readonly ITokenService _tokenService;

        public Login(FWD_CompContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        public async Task<UserDTO> AddForwarder(ForwarderAddDTO forwarder)
        {
            bool flag = false;
            CommonEnum status = CommonEnum.UNKNOWN_ERROR;
            Forwarder newForwarder = null;

            //check if exists
            Forwarder fwd = await _context.Forwarders
                .Where(x => x.Name == forwarder.Login)
                .SingleOrDefaultAsync();

            if (forwarder.PassHash.Length < 6)
            {
                status = CommonEnum.INVALID_PASSWORD;
            }
            if (fwd != null)
            {
                if (fwd.Name.Equals(forwarder.Login))
                {
                    status = CommonEnum.INVALID_LOGIN;
                    //login taken
                }
                if (fwd.Surname.Equals(forwarder.Surname) && fwd.Prefix.Equals(forwarder.Prefix))
                {
                    status = CommonEnum.ALREADY_EXISTS;
                    //forwarder with given parameters already exists (surname + prefix)
                }
            }
            else
                flag = true;

            if (flag)
            {
                using var hmac = new HMACSHA512();

                newForwarder = new Forwarder
                {
                    Name = forwarder.Login,
                    Surname = forwarder.Surname,
                    Prefix = forwarder.Prefix,
                    PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(forwarder.PassHash)),
                    PassSalt = hmac.Key
                };

                await _context.AddAsync(newForwarder);

                flag = await _context.SaveChangesAsync() > 0;
                status = CommonEnum.SUCCESSFULLY_ADDED;
                //forwarder successfully added

                if (!flag)
                {
                    status = CommonEnum.CANNOT_SAVE;
                    //could not save changes
                }

                hmac.Dispose();
                return new UserDTO
                {
                    UserName = newForwarder.Name,
                    Token = _tokenService.CreateToken(newForwarder),
                    Status = status
                };
            }
            return new UserDTO 
            { 
                UserName = "",
                Token = "",
                Status = status
            };
        }
        public async Task<UserDTO> UserLoginStatus(UserLoginDTO userDTO)
        {
            CommonEnum status = CommonEnum.UNKNOWN_ERROR;
            var userFromDb = await _context.Forwarders
                .SingleOrDefaultAsync(x => x.Name == userDTO.Login);

            if (userFromDb == null)
            {
                status = CommonEnum.INVALID_LOGIN;
                return new UserDTO
                {
                    UserName = "Empty",
                    Token = "Empty",
                    Status = status
                };
            }
            else
            {
                using var hmac = new HMACSHA512(userFromDb.PassSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != userFromDb.PassHash[i])
                        status = CommonEnum.INVALID_PASSWORD;
                }

                if (status != CommonEnum.INVALID_PASSWORD)
                    status = CommonEnum.SUCCESSFULLY_FOUND;
            }
            return new UserDTO
            {
                UserName = userFromDb.Name,
                Token = _tokenService.CreateToken(userFromDb),
                Status = status
            };
        }
    }
}
