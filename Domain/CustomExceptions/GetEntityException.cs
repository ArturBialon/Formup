namespace Domain.CustomExceptions
{
    public class GetEntityException : Exception
    {
        public GetEntityException() : base("Could not find record in database")
        {
        }

        public GetEntityException(string message)
            : base(message)
        {
        }

        public GetEntityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
