using Application.Common.Results;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkCaseItems.Commands
{
    public record DeleteWorkCaseItemCommand(Guid WorkCaseItemId) : IRequest<AppResult<Unit>>;
    public class DeleteWorkCaseItemHandler(FormupContext context)
        : IRequestHandler<DeleteWorkCaseItemCommand, AppResult<Unit>>
    {
        private readonly FormupContext _context = context;

        public async Task<AppResult<Unit>> Handle(DeleteWorkCaseItemCommand request, CancellationToken ct)
        {
            var workCaseItem = await _context.WorkCaseItems
                .Include(x => x.Invoice)
                .Include(x => x.Costs)
                .FirstOrDefaultAsync(x => x.Id.Value == request.WorkCaseItemId, ct);

            if (workCaseItem == null) return AppResult<Unit>.Failure("WORK_CASE_ITEM.NOT_FOUND");
            if (workCaseItem.IsInvoiced) return AppResult<Unit>.Failure("WORK_CASE_ITEM.CANNOT_DELETE_IS_INVOICED");
            if (workCaseItem.HasCosts) return AppResult<Unit>.Failure("WORK_CASE_ITEM.CANNOT_DELETE_WITH_COSTS");


            _context.WorkCaseItems.Remove(workCaseItem);
            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
