using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Access;
using Infrastructure.Context;
using MediatR;

namespace Application.Features.Users.Queries
{
    public record GetCurrentUserQuery() : IRequest<AppResult<UserResponse>>;

    public class GetCurrentUserQueryHandler(FormupContext context, ICurrentUserService userService)
        : IRequestHandler<GetCurrentUserQuery, AppResult<UserResponse>>
    {
        private readonly FormupContext _context = context;
        private readonly ICurrentUserService _currentUserService = userService;

        public async Task<AppResult<UserResponse>> Handle(GetCurrentUserQuery request, CancellationToken ct)
        {
            var userId = _currentUserService.UserId;
            if (userId == null) return AppResult<UserResponse>.Failure("COMMON.NOT_FOUND");

            var user = await _context.Users.FindAsync([userId], cancellationToken: ct);
            if (user == null) return AppResult<UserResponse>.Failure("UNAUTHORIZED_ERROR");

            var response = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.Name,
                Role = user.Role.ToString(),
                Token = new ResponseToken { AccessToken = _currentUserService.Token ?? string.Empty }
            };

            if (response.Token.Equals(string.Empty)) return AppResult<UserResponse>.Failure("UNAUTHORIZED_ERROR");

            return AppResult<UserResponse>.Success(response);
        }
    }
}
