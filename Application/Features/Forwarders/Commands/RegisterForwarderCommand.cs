using Application.DTOs.Response;
using Domain.CustomExceptions;
using Domain.Interfaces.UserAccessService;
using Domain.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Forwarders.Commands
{
    public record RegisterForwarderCommand(string Name, string Surname, string Prefix, string Password)
    : IRequest<UserResponseDTO>;

    public class RegisterForwarderHandler(FormupContext context, ITokenService tokenService) : IRequestHandler<RegisterForwarderCommand, UserResponseDTO>
    {
        private readonly FormupContext _context = context;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<UserResponseDTO> Handle(RegisterForwarderCommand request, CancellationToken ct)
        {
            if (request.Password.Length < 6)
                throw new RegistrationException("Password is too short (min 30 characters)");
            if (request.Password.Length > 30)
                throw new RegistrationException("Password is too long (max 30 characters)");

            bool exists = await _context.Forwarders.AnyAsync(x =>
                x.Name.Equals(request.Name, StringComparison.CurrentCultureIgnoreCase) ||
                x.Prefix.Equals(request.Prefix, StringComparison.CurrentCultureIgnoreCase), ct);

            if (exists)
                throw new RegistrationException("Login or prefix taken");

            using var hmac = new HMACSHA512();
            var newForwarder = new Forwarder
            {
                Name = request.Name,
                Surname = request.Surname,
                Prefix = request.Prefix.ToUpper(),
                PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
                PassSalt = hmac.Key
            };

            _context.Forwarders.Add(newForwarder);

            if (await _context.SaveChangesAsync(ct) > 0)
            {
                return new UserResponseDTO
                {
                    UserName = newForwarder.Name,
                    Token = new ResponseTokenDTO { AccessToken = _tokenService.CreateToken(newForwarder) }
                };
            }

            throw new RegistrationException();
        }
    }
}
