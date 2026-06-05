namespace Application.DTOs.Response
{
    public class WorkCaseListDTO()
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required decimal Amount { get; set; }
        public required string Relation { get; set; }
        public required string ForwarderName { get; set; }
        public required string ClientName { get; set; }
    }

}
