namespace Application.DTOs.Response
{
    public class InvoiceDetailResponse
    {
        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public decimal Tax { get; set; }

        public Guid WorkCaseId { get; set; }
        public string WorkCaseRelation { get; set; } = null!;
        public Guid ClientId { get; set; }
        public string ClientName { get; set; } = null!;
        public string ForwarderName { get; set; } = null!;

        public List<InvoiceItemDetail> Items { get; set; } = [];
    }

    public class InvoiceItemDetail
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
