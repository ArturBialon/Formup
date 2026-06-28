using Application.Common.Results;
using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Clients.Queries
{
    public record GetClientByIdQuery(Guid Id) : IRequest<IAppResult<ClientDetailResponse>>;

    public class GetClientByIdQueryHandler(FormupContext context)
        : IRequestHandler<GetClientByIdQuery, IAppResult<ClientDetailResponse>>
    {
        private readonly FormupContext _context = context;

        public async Task<IAppResult<ClientDetailResponse>> Handle(GetClientByIdQuery request, CancellationToken ct)
        {
            var client = await _context.Clients
                .AsNoTracking()
                .Where(x => x.Id.Value == request.Id)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Tax,
                    c.Street,
                    c.Zip,
                    c.Coutry,
                    c.Currency,
                    c.Credit,
                    WorkCases = c.WorkCases
                        .Select(w => new
                        {
                            Id = w.Id.Value,
                            w.Name
                        })
                        .ToList(),
                    Invoices = c.Invoices
                        .Select(i => new
                        {
                            Id = i.Id.Value,
                            i.InvoiceNumber
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (client == null)
            {
                return AppResult<ClientDetailResponse>.Failure("CLIENT.NOT_FOUND");
            }

            var clientDto = new ClientDetailResponse
            {
                Id = client.Id.Value,
                Tax = client.Tax,
                Name = client.Name,
                Street = client.Street,
                Zip = client.Zip,
                Coutry = client.Coutry,
                Credit = client.Credit,
                Currency = client.Currency,
                WorkCases = client.WorkCases.ToDictionary(wc => wc.Id, wc => wc.Name),
                Invoices = client.Invoices.ToDictionary(inv => inv.Id, inv => inv.InvoiceNumber)
            };
            return AppResult<ClientDetailResponse>.Success(clientDto);
        }
    }
}
