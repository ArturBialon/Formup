using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries
{
    public record GetUserByIdQuery(Guid Id) : IRequest<AppResult<UserDetailResponse>>;

    public class GetUserByIdQueryHandler(FormupContext context)
        : IRequestHandler<GetUserByIdQuery, AppResult<UserDetailResponse>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<UserDetailResponse>> Handle(GetUserByIdQuery request, CancellationToken ct)
        {
            var userDto = await _context.Users
                .AsNoTracking()
                .Where(x => x.Id.Value == request.Id)
                .Select(user => new UserDetailResponse
                {
                    Id = user.Id.Value,
                    Email = user.Email,
                    Name = user.Name,
                    Surname = user.Surname,
                    Prefix = user.Prefix,
                    Role = user.Role.ToString(),
                    IsActive = user.IsActive
                })
                .FirstOrDefaultAsync(ct);

            if (userDto == null)
            {
                return AppResult<UserDetailResponse>.Failure("USER.NOT_FOUND");
            }

            return AppResult<UserDetailResponse>.Success(userDto);
        }
    }
}
