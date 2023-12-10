namespace Domain.DTO.Request
{
    public class CostRequestDTO
    {
        public decimal Amount { get; set; }
        public int Tax { get; set; }
        public required string Name { get; set; }
        public int CasesId { get; set; }
        public int ServiceProvidersId { get; set; }
        public required string ErrorMessage { get; set; }
    }
}
