namespace Domain.DTO.Response
{
    public class ForwarderResponseDTO
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public required string Surname { get; set; }
        public required string Prefix { get; set; }
        public required string ErrorMessage { get; set; }
    }
}
