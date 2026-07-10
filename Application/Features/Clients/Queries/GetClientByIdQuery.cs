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
                .Where(x => x.Id.Equals(request.Id))
                .Select(c => new
                {
                    c.Id,
                    c.Tax,
                    c.Name,
                    c.Country,
                    c.City,
                    c.Zip,
                    c.Street,
                    c.HouseNumber,
                    c.ApartmentNumber,
                    c.Email,
                    c.PhoneNumber,
                    c.Credit,
                    c.Currency,
                    c.IsActive,
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
                Country = client.Country,
                City = client.City,
                Zip = client.Zip,
                Street = client.Street,
                HouseNumber = client.HouseNumber,
                ApartmentNumber = client.ApartmentNumber,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Credit = client.Credit,
                Currency = client.Currency,
                IsActive = client.IsActive,
                WorkCases = client.WorkCases.ToDictionary(wc => wc.Id, wc => wc.Name),
                Invoices = client.Invoices.ToDictionary(inv => inv.Id, inv => inv.InvoiceNumber)
            };
            return AppResult<ClientDetailResponse>.Success(clientDto);
        }
    }
}
