namespace Application.DTOs.Response
{
    public class ResponseTokenDTO
    {
        public required string AccessToken { get; set; }
        public string TokenType = "Bearer";
    }
}
