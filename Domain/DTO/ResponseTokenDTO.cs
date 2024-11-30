namespace Domain.DTO
{
    public class ResponseTokenDTO
    {
        public required string AccessToken { get; set; }
        public string TokenType = "Bearer";
    }
}
