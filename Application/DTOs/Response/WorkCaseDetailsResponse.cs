namespace Application.DTOs.Response
{
    public class WorkCaseDetailsResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountInPln { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public string Currency { get; set; } = "PLN";
        public required string Relation { get; set; }
        public Guid ForwarderId { get; set; }
        public string ForwarderName { get; set; } = string.Empty;
        public Guid ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public bool IsAbandoned { get; set; }
    }
}
