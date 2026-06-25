using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Invoices.Queries
{
    public record GetInvoiceByIdQuery(Guid InvoiceId) : IRequest<AppResult<InvoiceDetailResponse>>;

    public class GetInvoiceByIdHandler(FormupContext context)
        : IRequestHandler<GetInvoiceByIdQuery, AppResult<InvoiceDetailResponse>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<InvoiceDetailResponse>> Handle(GetInvoiceByIdQuery request, CancellationToken ct)
        {
            var invoiceDetail = await _context.Invoices
                .AsNoTracking()
                .Where(i => i.Id.Value == request.InvoiceId)
                .Select(i => new InvoiceDetailResponse
                {
                    Id = i.Id.Value,
                    InvoiceNumber = i.InvoiceNumber,
                    Amount = i.Amount,
                    Currency = i.Currency,
                    IssueDate = i.IssueDate,
                    ServiceDate = i.ServiceDate,
                    Tax = i.Tax,
                    IsAbandoned = i.IsAbandoned,

                    WorkCaseId = i.WorkCase.Id.Value,
                    WorkCaseRelation = i.WorkCase.Relation,
                    ClientId = i.Client.Id.Value,
                    ClientName = i.Client.Name,
                    ForwarderName = i.WorkCase.Forwarder.Name + " " + i.WorkCase.Forwarder.Surname,

                    Items = i.WorkCaseItems
                        .Select(wi => new InvoiceItemDetail
                        {
                            ItemId = wi.Id.Value,
                            Name = wi.Name,
                            Amount = wi.Amount,
                            Currency = wi.Currency
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (invoiceDetail == null)
            {
                return AppResult<InvoiceDetailResponse>.Failure("INVOICE.NOT_FOUND");
            }

            return AppResult<InvoiceDetailResponse>.Success(invoiceDetail);
        }
    }
}
