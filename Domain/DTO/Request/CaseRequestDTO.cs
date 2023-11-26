using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.Request
{
    public class CaseRequestDTO
    {
        public required string Name { get; set; }
        public int Amount { get; set; }
        public required string Relation { get; set; }
        public int ForwarderId { get; set; }
    }
}
