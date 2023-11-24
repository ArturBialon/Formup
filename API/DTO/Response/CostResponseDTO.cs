namespace API.DTO.Response
{
    public class CostResponseDTO
    {
        public decimal AmountNet { get; set; }
        public decimal AmountBrut { get; set; }
        public int Tax { get; set; }
        public string Name { get; set; }
    }
}
