namespace Domain.DTO.Response
{
    public class CaseResponseDTO
    {
        public required string Name { get; set; }
        public int Amount { get; set; }
        public required string Relation { get; set; }
        public required string ClientName { get; set; }
        public required string ForwarderName { get; set; }
        public int? NumberOfInvoices { get; set; }
        public decimal? TotalCosts { get; set; }
        public decimal? TotalSales { get; set; }
        public required string ErrorMessage { get; set; }
    }
}
