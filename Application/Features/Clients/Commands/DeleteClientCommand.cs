using Application.Common.Results;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Clients.Commands
{
    public record DeleteClientCommand(Guid Id) : IRequest<IAppResult<Unit>>;

    public class DeleteClientCommandHandler(FormupContext context) : IRequestHandler<DeleteClientCommand, IAppResult<Unit>>
    {
        private readonly FormupContext _context = context;

        public async Task<IAppResult<Unit>> Handle(DeleteClientCommand request, CancellationToken ct)
        {
            var client = await _context.Clients
                .Include(x => x.Invoices)
                .Include(x => x.WorkCases)
                .FirstOrDefaultAsync(x => x.Id.Value == request.Id, ct);

            if (client == null)
            {
                return AppResult<Unit>.Failure("CLIENT.NOT_FOUND");
            }

            if (client.Invoices.Count != 0)
            {
                return AppResult<Unit>.Failure("CLIENT.HAS_INVOICES");
            }

            if (client.WorkCases.Count != 0)
            {
                return AppResult<Unit>.Failure("CLIENT.HAS_WORK_CASES");
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync(ct);

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
