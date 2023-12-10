namespace Domain.DTO.Request
{
    public class ForwarderRequestDTO
    {
        public required string Login { get; set; }
        public required string Surname { get; set; }
        public required string Prefix { get; set; }
        public required string PassHash { get; set; }
        public required string ErrorMessage { get; set; }

    }
}
