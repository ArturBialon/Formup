namespace Domain.CustomExceptions
{
    public class LoginException : Exception
    {
        public LoginException() : base("Invalid username or password.")
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
