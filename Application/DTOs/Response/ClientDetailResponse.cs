namespace Application.DTOs.Response
{
    public class ClientDetailResponse
    {
        public Guid Id { get; set; }
        public string Tax { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Zip { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string? ApartmentNumber { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal Credit { get; set; }
        public string Currency { get; set; } = null!;
        public bool IsActive { get; set; }

        public Dictionary<Guid, string> WorkCases { get; set; } = [];
        public Dictionary<Guid, string> Invoices { get; set; } = [];
    }
}
