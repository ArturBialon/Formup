namespace Application.Errors
{
    public class ApiException(int statusCode, string message = "null", string details = "null")
    {
        public int Status { get; set; } = statusCode;
        public string Message { get; set; } = message;
        public string Details { get; set; } = details;
    }
}
