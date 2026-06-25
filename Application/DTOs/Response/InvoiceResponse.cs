namespace Application.DTOs.Response
{
    public class InvoiceResponse
    {
        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public decimal Tax { get; set; }
        public bool IsAbandoned { get; set; }
        public Guid WorkCaseId { get; set; }
        public Guid ClientId { get; set; }
        public List<Guid> InvoicedItemIds { get; set; } = [];
    }
}