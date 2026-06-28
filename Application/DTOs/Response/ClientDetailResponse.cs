namespace Application.DTOs.Response
{
    public class ClientDetailResponse
    {
        public Guid Id { get; set; }
        public string Tax { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Zip { get; set; } = null!;
        public string Coutry { get; set; } = null!;
        public decimal Credit { get; set; }
        public string Currency { get; set; } = null!;

        public Dictionary<Guid, string> WorkCases { get; set; } = [];
        public Dictionary<Guid, string> Invoices { get; set; } = [];
    }
}
