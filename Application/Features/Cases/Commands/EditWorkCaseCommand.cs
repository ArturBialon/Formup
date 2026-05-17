using Application.DTOs.Response;
using Infrastructure.Context;
using MediatR;

namespace Application.Features.Cases.Commands
{
    public record EditWorkCaseCommand(int Amount, string Relation, Guid ForwarderId) : IRequest<WorkCaseResponseDTO>;
    public class EditWorkCaseHandler(FormupContext context) : IRequestHandler<EditWorkCaseCommand, WorkCaseResponseDTO>
    {
        private readonly FormupContext _context = context;

        public Task<WorkCaseResponseDTO> Handle(EditWorkCaseCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
