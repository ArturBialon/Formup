
using Domain.Models;

namespace Application.DTOs.Response
{
    public class WorkCaseResponseDTO
    {
        public required string Name { get; set; }
        public int Amount { get; set; }
        public required string Relation { get; set; }
        public bool IsAbandoned { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
