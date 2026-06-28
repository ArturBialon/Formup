namespace Application.Common.CurrencyServices
{
    public record CurrencyConversionInput(Guid ItemId, decimal Amount, string Currency);

    public class CurrencyConversionResult
    {
        public decimal TotalTargetAmount { get; set; }
        public string TargetCurrency { get; set; } = null!;
        public List<ConvertedItemDetail> Details { get; set; } = [];
    }

    public class ConvertedItemDetail
    {
        public Guid ItemId { get; set; }
        public decimal OriginalAmount { get; set; }
        public string OriginalCurrency { get; set; } = null!;
        public decimal ConvertedAmount { get; set; }
        public decimal ExchangeRateUsed { get; set; }
    }
}
