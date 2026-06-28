namespace Application.DTOs.Response
{
    public class WorkCaseItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "PLN";
        public decimal Tax { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsInvoiced { get; set; }
        public Guid? InvoiceId { get; set; }
        public ICollection<CostResponse> Costs { get; set; } = [];
    }
}
