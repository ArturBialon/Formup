using MediatR;

namespace Application.Features.Cases.Commands
{
    public record CreateWorkCaseCommand(string Name, int Amount, string Relation)
    {
    }
}
