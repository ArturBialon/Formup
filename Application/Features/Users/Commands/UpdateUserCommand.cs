using Application.Common.Results;
using Domain.Enums;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Users.Commands
{
    public record UpdateUserCommand(
        Guid UserId,
        string Email,
        string Name,
        string Surname,
        string Prefix,
        UserRole Role,
        bool IsActive,
        string? Password
    ) : IRequest<AppResult<Unit>>;

    public class UpdateUserHandler(FormupContext context, IConfiguration _config) : IRequestHandler<UpdateUserCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;
        private readonly IConfiguration _config = _config;

        public async Task<AppResult<Unit>> Handle(UpdateUserCommand request, CancellationToken ct)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id.Value == request.UserId, ct);

            if (user == null)
            {
                return AppResult<Unit>.Failure("USER.NOT_FOUND");
            }

            var normalizedEmail = request.Email.ToLower().Trim();
            var normalizedPrefix = request.Prefix.ToUpper().Trim();

            bool conflictExists = await _context.Users.AnyAsync(x =>
                x.Id.Value != request.UserId &&
                (x.Email == normalizedEmail || x.Prefix == normalizedPrefix), ct);

            if (conflictExists)
            {
                return AppResult<Unit>.Failure("USER.EMAIL_OR_PREFIX_ALREADY_TAKEN");
            }

            user.Email = normalizedEmail;
            user.Name = request.Name.Trim();
            user.Surname = request.Surname.Trim();
            user.Prefix = normalizedPrefix;
            user.Role = request.Role;
            user.IsActive = request.IsActive;

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var pepper = _config["PasswordPepper"];
                var passwordWithPepper = $"{request.Password}{pepper}";

                using var hmac = new HMACSHA512();
                user.PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
                user.PassSalt = hmac.Key;
            }

            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }

}
