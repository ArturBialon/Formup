namespace Domain.CustomExceptions
{
    public class InstanceException : Exception
    {
        public Guid ResourceId { get; }

        public InstanceException(string entityName, Guid id)
            : base($"Could not find instance of an '{entityName}', Guid: {id}")
        {
            ResourceId = id;
        }

        public InstanceException(string message, Guid id, bool customMessage)
            : base(message)
        {
            ResourceId = id;
        }
    }
}
