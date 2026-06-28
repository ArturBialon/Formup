using Application.Common.Results;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ServiceContractors.Commands
{
    public record DeleteServiceContractorCommand(Guid Id) : IRequest<AppResult<Unit>>;
    public class DeleteServiceContractorHandler(FormupContext context)
        : IRequestHandler<DeleteServiceContractorCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<Unit>> Handle(DeleteServiceContractorCommand request, CancellationToken ct)
        {
            var contractor = await _context.ServiceContractors
                .Include(x => x.Costs)
                .FirstOrDefaultAsync(x => x.Id.Value == request.Id, ct);

            if (contractor == null)
            {
                return AppResult<Unit>.Failure("CONTRACTOR.NOT_FOUND");
            }

            if (contractor.Costs.Count != 0)
            {
                return AppResult<Unit>.Failure("CONTRACTOR.VALIDATION.CANNOT_DELETE_WITH_INVOICES");
            }

            _context.ServiceContractors.Remove(contractor);
            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
