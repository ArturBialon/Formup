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
    ) : IRequest<AppResult<PagedResult<InvoiceResponse>>>;

    public class GetInvoicesQueryHandler(FormupContext context)
        : IRequestHandler<GetInvoicesQuery, AppResult<PagedResult<InvoiceResponse>>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<PagedResult<InvoiceResponse>>> Handle(GetInvoicesQuery request, CancellationToken ct)
        {
            var query = _context.Invoices.AsNoTracking().AsQueryable();

            if (request.ClientId.HasValue)
                query = query.Where(x => x.Client.Id.Value == request.ClientId.Value);

            if (request.ForwarderId.HasValue)
                query = query.Where(x => x.WorkCase.Forwarder.Id.Value == request.ForwarderId.Value);

            if (request.MinAmount.HasValue)
                query = query.Where(x => x.Amount >= request.MinAmount.Value);

            if (request.MaxAmount.HasValue)
                query = query.Where(x => x.Amount <= request.MaxAmount.Value);

            if (request.IssueDateFrom.HasValue)
                query = query.Where(x => x.IssueDate >= request.IssueDateFrom.Value);

            if (request.IssueDateTo.HasValue)
                query = query.Where(x => x.IssueDate <= request.IssueDateTo.Value);

            if (request.ServiceDateFrom.HasValue)
                query = query.Where(x => x.ServiceDate >= request.ServiceDateFrom.Value);

            if (request.ServiceDateTo.HasValue)
                query = query.Where(x => x.ServiceDate <= request.ServiceDateTo.Value);

            if (request.TaxRate.HasValue)
                query = query.Where(x => x.Tax == request.TaxRate.Value);

            if (!string.IsNullOrWhiteSpace(request.Relation))
                query = query.Where(x => x.WorkCase.Relation.Contains(request.Relation));

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(x => x.IssueDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(invoice => new InvoiceResponse
                {
                    Id = invoice.Id.Value,
                    InvoiceNumber = invoice.InvoiceNumber,
                    Amount = invoice.Amount,
                    Currency = invoice.Currency,
                    IssueDate = invoice.IssueDate,
                    ServiceDate = invoice.ServiceDate,
                    Tax = invoice.Tax,
                    WorkCaseId = invoice.WorkCase.Id.Value,
                    ClientId = invoice.Client.Id.Value,
                    InvoicedItemIds = invoice.WorkCaseItems.Select(item => item.Id.Value).ToList()
                })
                .ToListAsync(ct);

            var pagedResult = new PagedResult<InvoiceResponse>(items, totalCount, request.PageNumber, request.PageSize);

            return AppResult<PagedResult<InvoiceResponse>>.Success(pagedResult);
        }
    }
}
