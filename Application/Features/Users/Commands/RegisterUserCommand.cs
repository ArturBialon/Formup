using Application.Common.Results;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Users.Commands
{
    public record RegisterUserCommand(
        string Email,
        string Name,
        string Surname,
        string Prefix,
        string Password,
        UserRole Role)
    : IRequest<AppResult<Unit>>;

    public class RegisterUserHandler(FormupContext context, IConfiguration config)
        : IRequestHandler<RegisterUserCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;
        private readonly IConfiguration _config = config;

        public async Task<AppResult<Unit>> Handle(RegisterUserCommand request, CancellationToken ct)
        {
            var normalizedEmail = request.Email.ToLower().Trim();
            var normalizedInitials = request.Prefix.ToUpper().Trim();

            bool userExists = await _context.Users.AnyAsync(x =>
                x.Email == normalizedEmail || x.Prefix == normalizedInitials, ct);

            if (userExists)
            {
                return AppResult<Unit>.Failure("REGISTRATION.EMAIL_OR_INITIALS_TAKEN");
            }

            var pepper = _config["PasswordPepper"];

            if (string.IsNullOrEmpty(pepper))
            {
                return AppResult<Unit>.Failure("REGISTRATION.SECURITY_CONFIGURATION_ERROR");
            }
            var passwordWithPepper = $"{request.Password}{pepper}";

            using var hmac = new HMACSHA512();

            var newUser = new User
            {
                Email = normalizedEmail,
                Name = request.Name.Trim(),
                Surname = request.Surname.Trim(),
                Prefix = normalizedInitials,
                Role = request.Role,
                PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordWithPepper)),
                PassSalt = hmac.Key,
                IsActive = true
            };

            _context.Users.Add(newUser);

            if (await _context.SaveChangesAsync(ct) > 0)
            {
                return AppResult<Unit>.Success(Unit.Value);
            }

            return AppResult<Unit>.Failure("REGISTRATION.DATABASE_ERROR");
        }
    }
}
