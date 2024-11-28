namespace Domain.CustomExceptions
{
    public class RegistrationException : Exception
    {
        public RegistrationException() : base("Unable to register.")
        {
        }

        public RegistrationException(string message) : base(message)
        {
        }

        public RegistrationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
