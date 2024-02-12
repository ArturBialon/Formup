namespace Domain.CustomExceptions
{
    public class LoginException : Exception
    {
        public LoginException() : base("Login failed due to invalid username or password.")
        {
        }

        public LoginException(string message) : base(message)
        {
        }

        public LoginException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
