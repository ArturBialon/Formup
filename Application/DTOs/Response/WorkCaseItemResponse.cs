namespace Application.DTOs.Response
{
    public class WorkCaseItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal AmountToInvoice { get; set; }
        public string InvoiceCurrencyCode { get; set; } = "PLN";
        public decimal Tax { get; set; }
        public decimal CostAmount { get; set; }
        public string CostCurrencyCode { get; set; } = "PLN";
        public DateTime CreatedAtUtc { get; set; }
        public bool IsInvoiced { get; set; }
        public Guid? InvoiceId { get; set; }
        public ICollection<CostResponse> Costs { get; set; } = [];
    }
}
