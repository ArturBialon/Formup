namespace API.DTO.Response
{
    public class CaseListResponseDTO
    {
        public string Name { get; set; }
        public string ClientName { get; set; }
        public string ForwarderName { get; set; }
        public int? NumberOfInvoices { get; set; }
        public decimal? TotalCosts { get; set; }
        public decimal? TotalSales { get; set; }
    }
}