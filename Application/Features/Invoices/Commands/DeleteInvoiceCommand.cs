using Application.Common.Results;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Invoices.Commands
{
    public record DeleteInvoiceCommand(Guid InvoiceId) : IRequest<AppResult<Unit>>;

    public class DeleteInvoiceHandler(FormupContext context) : IRequestHandler<DeleteInvoiceCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<Unit>> Handle(DeleteInvoiceCommand request, CancellationToken ct)
        {
            var invoice = await _context.Invoices
                .Include(x => x.WorkCaseItems)
                .FirstOrDefaultAsync(x => x.Id.Value == request.InvoiceId, ct);
            if (invoice == null) return AppResult<Unit>.Failure("INVOICE.NOT_FOUND");

            foreach (var item in invoice.WorkCaseItems)
            {
                item.Invoice = null;
            }

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
