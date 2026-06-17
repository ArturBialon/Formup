
namespace Application.DTOs.Response
{
    public class ServiceContractorResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Tax { get; set; }
        public required string Country { get; set; }
        public required string City { get; set; }
        public required string Zip { get; set; }
        public required string Street { get; set; }
        public required string HouseNumber { get; set; }
        public string? ApartmentNumber { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
