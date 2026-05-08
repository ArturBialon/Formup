namespace Domain.DTO.Response
{
    public class ForwarderResponseDTO
    {
        public Guid Id { get; set; }
        public required string Login { get; set; }
        public required string Surname { get; set; }
        public required string Prefix { get; set; }
    }
}
