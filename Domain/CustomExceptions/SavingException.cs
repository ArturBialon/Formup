namespace Domain.CustomExceptions
{
    public class SavingException : Exception
    {
        public SavingException() : base("Unable to save entity")
        {
        }

        public SavingException(string message) : base(message)
        {
        }

        public SavingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}