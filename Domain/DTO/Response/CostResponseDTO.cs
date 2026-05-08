namespace Domain.DTO.Response
{
    public class CostResponseDTO
    {
        public Guid Id { get; set; }
        public decimal AmountNet { get; set; }
        public decimal AmountBrut { get; set; }
        public int Tax { get; set; }
        public required string Name { get; set; }
    }
}
