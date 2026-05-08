using Infrastructure.Models;

namespace Domain.DTO.Request
{
    public class CostRequestDTO
    {
        public Cost.EntityId Id { get; set; }
        public decimal Amount { get; set; }
        public int Tax { get; set; }
        public required string Name { get; set; }
        public Guid WorkCasesId { get; set; }
        public Guid ServiceProvidersId { get; set; }
    }
}
