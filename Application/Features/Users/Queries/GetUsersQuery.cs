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
                    x.Name.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                    x.Surname.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                    x.Email.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                    x.Prefix.Contains(term, StringComparison.CurrentCultureIgnoreCase));
            }

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(x => x.Surname)
                .ThenBy(x => x.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
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

            var pagedResult = new PagedResult<UserListItemResponse>(items, totalCount, request.PageNumber, request.PageSize);


            return AppResult<PagedResult<UserListItemResponse>>.Success(pagedResult);
        }
    }
}
