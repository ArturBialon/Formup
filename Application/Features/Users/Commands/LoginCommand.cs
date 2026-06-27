using Application.Common.Results;
using Application.DTOs.Response;
using Domain.Interfaces.UserService;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Users.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<AppResult<UserResponse>>;

    public class LoginHandler(FormupContext context, ITokenService tokenService, IConfiguration config)
    : IRequestHandler<LoginCommand, AppResult<UserResponse>>
    {
        private readonly FormupContext _context = context;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IConfiguration _config = config;

        public async Task<AppResult<UserResponse>> Handle(LoginCommand request, CancellationToken ct)
        {
            var userFromDb = await _context.Users
                .SingleOrDefaultAsync(x => x.Email == request.Email.ToLower().Trim(), ct);

            if (userFromDb == null || !userFromDb.IsActive)
            {
                return AppResult<UserResponse>.Failure("AUTH.INVALID_CREDENTIALS");
            }

            var pepper = _config["PasswordPepper"];
            if (string.IsNullOrEmpty(pepper))
            {
                return AppResult<UserResponse>.Failure("AUTH.SECURITY_CONFIGURATION_ERROR");
            }

            var passwordWithPepper = $"{request.Password}{pepper}";
            using var hmac = new HMACSHA512(userFromDb.PassSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordWithPepper));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != userFromDb.PassHash[i])
                {
                    return AppResult<UserResponse>.Failure("AUTH.INVALID_CREDENTIALS");
                }
            }

            var response = new UserResponse
            {
                Id = userFromDb.Id.Value,
                Email = userFromDb.Email,
                UserName = userFromDb.FullName,
                Role = userFromDb.Role.ToString(),
                Token = new ResponseToken
                {
                    AccessToken = _tokenService.CreateToken(userFromDb),
                    TokenType = "Bearer"
                }
            };

            return AppResult<UserResponse>.Success(response);
        }
    }
}
