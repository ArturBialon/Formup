namespace Application.DTOs.Response
{
    public class WorkCaseResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal Amount { get; set; }
        public required string Relation { get; set; }
        public bool IsAbandoned { get; set; } = false;
        public Guid ForwarderId { get; set; }
        public string ForwarderName { get; set; } = string.Empty;
        public Guid ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
