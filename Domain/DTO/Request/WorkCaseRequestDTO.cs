using Infrastructure.Models;

namespace Domain.DTO.Request
{
    public class WorkCaseRequestDTO
    {
        public WorkCase.EntityId Id { get; set; }
        public required string Name { get; set; }
        public int Amount { get; set; }
        public required string Relation { get; set; }
        public Guid ForwarderId { get; set; }
    }
}
