using Application.Common.Results;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Invoices.Commands
{
    public record AbandonInvoiceCommand(Guid InvoiceId) : IRequest<AppResult<Unit>>;

    public class DeleteInvoiceHandler(FormupContext context) : IRequestHandler<AbandonInvoiceCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<Unit>> Handle(AbandonInvoiceCommand request, CancellationToken ct)
        {
            var invoice = await _context.Invoices
                .Include(x => x.WorkCaseItems)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.InvoiceId), ct);

            if (invoice == null) return AppResult<Unit>.Failure("INVOICE.NOT_FOUND");
            if (invoice.IsPaid) return AppResult<Unit>.Failure("INVOICE.CANNOT_DELETE_PAID");

            foreach (var item in invoice.WorkCaseItems)
            {
                item.Invoice = null;
            }

            invoice.IsAbandoned = true;
            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
