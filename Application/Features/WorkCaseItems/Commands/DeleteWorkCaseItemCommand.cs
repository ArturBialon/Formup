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
                .Include(x => x.Cost)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.WorkCaseItemId), ct);

            if (workCaseItem == null) return AppResult<Unit>.Failure("WORK_CASE_ITEM.NOT_FOUND");
            if (workCaseItem.Invoice != null) return AppResult<Unit>.Failure("WORK_CASE_ITEM.CANNOT_DELETE_IS_INVOICED");
            if (workCaseItem.Cost != null) return AppResult<Unit>.Failure("WORK_CASE_ITEM.CANNOT_DELETE_WITH_COST");


            _context.WorkCaseItems.Remove(workCaseItem);
            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
