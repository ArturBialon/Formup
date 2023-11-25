namespace Infrastructure.DTO.Response

{
    public class CaseResponseDTO
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Relation { get; set; }
        public string ClientName { get; set; }
        public string ForwarderName { get; set; }
        public int? NumberOfInvoices { get; set; }
        public decimal? TotalCosts { get; set; }
        public decimal? TotalSales { get; set; }
    }
}
