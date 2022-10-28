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

        public Login(FWD_CompContext context)
        {
            _context = context;
        }
        public async Task<CommonEnum> UserLogin(UserLoginDTO userDTO)
        {
            CommonEnum status = CommonEnum.UNKNOWN_ERROR;
            var userFromDb = await _context.Forwarders
                .SingleOrDefaultAsync(x => x.Name == userDTO.Login);

            if (userFromDb == null)
            {
                status = CommonEnum.INVALID_LOGIN;
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
            return status;
        }
    }
}
