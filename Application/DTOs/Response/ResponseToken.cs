namespace Application.DTOs.Response
{
    public class ResponseToken
    {
        public required string AccessToken { get; set; }
        public string TokenType = "Bearer";
    }
}
