namespace Domain.DTO
{
    public class ClientDTO
    {
        public int Id { get; set; }
        public required string Tax { get; set; }
        public required string Name { get; set; }
        public required string Street { get; set; }
        public required string Zip { get; set; }
        public required string Coutry { get; set; }
        public decimal Credit { get; set; }
    }
}
