using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Invoices.Queries
{
    public record GetInvoicesQuery(
        int PageNumber = 1,
        int PageSize = 20,
        Guid? ClientId = null,
        Guid? ForwarderId = null,
        DateTime? IssueDateFrom = null,
        DateTime? IssueDateTo = null,
        DateTime? ServiceDateFrom = null,
        DateTime? ServiceDateTo = null,
        string? Relation = null,
        decimal? TaxRate = null,
        decimal? MinAmount = null,
        decimal? MaxAmount = null
    ) : IRequest<AppResult<PagedResult<InvoiceDetailResponse>>>;

    public class GetInvoicesQueryHandler(FormupContext context)
        : IRequestHandler<GetInvoicesQuery, AppResult<PagedResult<InvoiceDetailResponse>>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<PagedResult<InvoiceDetailResponse>>> Handle(GetInvoicesQuery request, CancellationToken ct)
        {
            var optimalPageSize = Math.Clamp(request.PageSize, 1, 1000);
            var query = _context.Invoices.AsNoTracking().AsQueryable();

            if (request.ClientId.HasValue)
                query = query.Where(x => x.Client.Id.Equals(request.ClientId));

            if (request.ForwarderId.HasValue)
                query = query.Where(x => x.WorkCase.Forwarder.Id.Equals(request.ForwarderId));

            if (request.MinAmount.HasValue)
                query = query.Where(x => x.Amount >= request.MinAmount.Value);

            if (request.MaxAmount.HasValue)
                query = query.Where(x => x.Amount <= request.MaxAmount.Value);

            if (request.IssueDateFrom.HasValue)
                query = query.Where(x => x.IssueDateUtc >= request.IssueDateFrom.Value);

            if (request.IssueDateTo.HasValue)
                query = query.Where(x => x.IssueDateUtc <= request.IssueDateTo.Value);

            if (request.ServiceDateFrom.HasValue)
                query = query.Where(x => x.ServiceDateUtc >= request.ServiceDateFrom.Value);

            if (request.ServiceDateTo.HasValue)
                query = query.Where(x => x.ServiceDateUtc <= request.ServiceDateTo.Value);

            if (request.TaxRate.HasValue)
                query = query.Where(x => x.Tax == request.TaxRate.Value);

            if (!string.IsNullOrWhiteSpace(request.Relation))
                query = query.Where(x => x.WorkCase.Relation.Contains(request.Relation));

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(x => x.IssueDateUtc)
                .Skip((request.PageNumber - 1) * optimalPageSize)
                .Take(optimalPageSize)
                .Select(invoice => new InvoiceDetailResponse
                {
                    Id = invoice.Id.Value,
                    InvoiceNumber = invoice.InvoiceNumber,
                    Amount = invoice.Amount,
                    Currency = invoice.CurrencyCode,
                    IssueDateUtc = invoice.IssueDateUtc,
                    ServiceDateUtc = invoice.ServiceDateUtc,
                    Tax = invoice.Tax,
                    IsAbandoned = invoice.IsAbandoned,
                    IsPaid = invoice.IsPaid,
                    ClientName = invoice.Client.Name,
                    ForwarderName = invoice.WorkCase.Forwarder.Name
                })
                .ToListAsync(ct);

            var pagedResult = new PagedResult<InvoiceDetailResponse>(items, totalCount, request.PageNumber, optimalPageSize);

            return AppResult<PagedResult<InvoiceDetailResponse>>.Success(pagedResult);
        }
    }
}
