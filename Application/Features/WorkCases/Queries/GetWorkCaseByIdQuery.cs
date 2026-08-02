using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCases.Queries
{
    public record GetWorkCaseByIdQuery(Guid Id) : IRequest<AppResult<WorkCaseDetailsResponse>>;

    public class GetWorkCaseByIdHandler(FormupContext context) : IRequestHandler<GetWorkCaseByIdQuery, AppResult<WorkCaseDetailsResponse>>
    {
        public async Task<AppResult<WorkCaseDetailsResponse>> Handle(GetWorkCaseByIdQuery request, CancellationToken ct)
        {
            var result = await context.WorkCases
                .AsNoTracking()
                .Where(x => x.Id.Equals(request.Id))
                .Select(x => new WorkCaseDetailsResponse
                {
                    Id = x.Id.Value,
                    Name = x.Name,
                    Amount = x.Amount,
                    AmountInPln = x.AmountInPln,
                    CreatedAtUtc = x.CreatedAtUtc,
                    Currency = x.CurrencyCode,
                    Relation = x.Relation,
                    ForwarderId = x.Forwarder.Id.Value,
                    ForwarderName = $"{x.Forwarder.Name} {x.Forwarder.Surname}",
                    ClientId = x.Client.Id.Value,
                    ClientName = x.Client.Name,
                    IsAbandoned = x.IsAbandoned,
                    IsCompleted = x.IsCompleted,
                    ClientResponse = x.Client == null ? null : new ClientResponse
                    {
                        Id = x.Client.Id.Value,
                        Tax = x.Client.Tax,
                        Name = x.Client.Name,
                        Country = x.Client.Country,
                        City = x.Client.City,
                        Zip = x.Client.Zip,
                        Street = x.Client.Street,
                        HouseNumber = x.Client.HouseNumber,
                        ApartmentNumber = x.Client.ApartmentNumber,
                        Email = x.Client.Email,
                        PhoneNumber = x.Client.PhoneNumber,
                        Credit = x.Client.Credit,
                        Currency = x.Client.CurrencyCode,
                        IsActive = x.Client.IsActive
                    },
                    InvoiceResponseList = x.Invoices
                    .Where(x => !x.IsAbandoned)
                    .Select(invoice => new InvoiceResponse
                    {
                        Id = invoice.Id.Value,
                        InvoiceNumber = invoice.InvoiceNumber,
                        Amount = invoice.Amount,
                        Currency = invoice.CurrencyCode,
                        IssueDateUtc = invoice.IssueDateUtc,
                        ServiceDateUtc = invoice.ServiceDateUtc,
                        Tax = invoice.Tax,
                        IsAbandoned = invoice.IsAbandoned,
                        WorkCaseId = invoice.WorkCase.Id.Value,
                        ClientId = invoice.Client.Id.Value
                    }).ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (result == null) return AppResult<WorkCaseDetailsResponse>.Failure("WORK_CASE.NOT_FOUND");

            return AppResult<WorkCaseDetailsResponse>.Success(result);
        }
    }
}