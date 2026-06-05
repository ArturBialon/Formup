using Application.DTOs.Response;
using Domain.CustomExceptions;
using Domain.Interfaces.UserService;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Forwarders.Commands
{
    public record LoginCommand(string Name, string Password) : IRequest<UserResponseDTO>;

    public class LoginHandler(FormupContext context, ITokenService tokenService) : IRequestHandler<LoginCommand, UserResponseDTO>
    {
        private readonly FormupContext _context = context;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<UserResponseDTO> Handle(LoginCommand request, CancellationToken ct)
        {
            var userFromDb = await _context.Forwarders
            .SingleOrDefaultAsync(x => x.Name == request.Name, ct);

            if (userFromDb == null)
            {
                throw new LoginException();
            }
            else
            {
                using var hmac = new HMACSHA512(userFromDb.PassSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != userFromDb.PassHash[i])
                        throw new LoginException();
                }

                hmac.Dispose();

                return new UserResponseDTO
                {
                    UserName = userFromDb.Name,
                    Token = new ResponseTokenDTO { AccessToken = _tokenService.CreateToken(userFromDb), TokenType = "Bearer" }
                };
            }
        }
    }
}
