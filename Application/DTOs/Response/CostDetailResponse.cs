namespace Application.DTOs.Response
{
    public record CostDetailResponse
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "PLN";
        public decimal Tax { get; set; }
        public required string Name { get; set; }
        public required DateTime IssueDate { get; set; }
        public required DateTime ServiceDate { get; set; }
        public string? DocumentUrl { get; set; }

        public Guid WorkCaseItemId { get; set; }
        public Guid ServiceContractorId { get; set; }
    }
}
