namespace Application.DTOs.Response
{
    public class CostResponse
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "PLN";
        public decimal Tax { get; set; }
        public required string Name { get; set; }
        public required DateTime IssueDate { get; set; }
        public required DateTime ServiceDate { get; set; }
    }
}
