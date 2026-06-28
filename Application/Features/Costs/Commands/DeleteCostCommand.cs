using Application.Common.FileStorage;
using Application.Common.Results;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Features.Costs.Commands
{
    public record DeleteCostCommand(Guid Id) : IRequest<IAppResult<Unit>>;

    public class DeleteCostCommandHandler(
        FormupContext context,
        ILogger logger,
        IFileStorageService fileStorageService) : IRequestHandler<DeleteCostCommand, IAppResult<Unit>>
    {
        private readonly FormupContext _context = context;
        private readonly ILogger _logger = logger;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<IAppResult<Unit>> Handle(DeleteCostCommand request, CancellationToken ct)
        {
            var cost = await _context.Costs
                .FirstOrDefaultAsync(x => x.Id.Value == request.Id, ct);

            if (cost == null)
            {
                return AppResult<Unit>.Failure("COST.NOT_FOUND");
            }

            string? fileUrlToDelete = cost.DocumentUrl;

            _context.Costs.Remove(cost);
            await _context.SaveChangesAsync(ct);

            if (!string.IsNullOrEmpty(fileUrlToDelete))
            {
                try
                {
                    await _fileStorageService.DeleteFileAsync(fileUrlToDelete, ct);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, ex.Message);
                    // ponieważ rekord z bazy zniknął pomyślnie.
                }
            }

            return AppResult<Unit>.Success(Unit.Value);
        }
    }
}
