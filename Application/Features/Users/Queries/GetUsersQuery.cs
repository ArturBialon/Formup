using Application.Common.Results;
using Application.DTOs.Response;
using Domain.Enums;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries
{
    public record GetUsersQuery(
        int PageNumber = 1,
        int PageSize = 20,
        string? SearchTerm = null,
        UserRole? Role = null,
        bool? IsActive = null
    ) : IRequest<AppResult<PagedResult<UserListItemResponse>>>;

    public class GetUsersQueryHandler(FormupContext context)
        : IRequestHandler<GetUsersQuery, AppResult<PagedResult<UserListItemResponse>>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<PagedResult<UserListItemResponse>>> Handle(GetUsersQuery request, CancellationToken ct)
        {
            var optimalPageSize = Math.Clamp(request.PageSize, 1, 1000);
            var query = _context.Users.AsNoTracking().AsQueryable();

            if (request.Role.HasValue)
            {
                query = query.Where(x => x.Role == request.Role.Value);
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLower();

                query = query.Where(x =>
                    x.Name.Contains(term) ||
                    x.Surname.Contains(term) ||
                    x.Email.Contains(term) ||
                    x.Prefix.Contains(term));
            }

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(x => x.Id)
                .ThenBy(x => x.Name)
                .Skip((request.PageNumber - 1) * optimalPageSize)
                .Take(optimalPageSize)
                .Select(user => new UserListItemResponse
                {
                    Id = user.Id.Value,
                    Email = user.Email,
                    Name = user.Name,
                    Surname = user.Surname,
                    Prefix = user.Prefix,
                    Role = user.Role.ToString(),
                    IsActive = user.IsActive
                })
                .ToListAsync(ct);

            var pagedResult = new PagedResult<UserListItemResponse>(items, totalCount, request.PageNumber, optimalPageSize);


            return AppResult<PagedResult<UserListItemResponse>>.Success(pagedResult);
        }
    }
}
