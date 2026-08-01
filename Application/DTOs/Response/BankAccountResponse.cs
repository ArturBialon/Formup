namespace Application.DTOs.Response
{
    public class BankAccountResponse
    {
        public required Guid Id { get; set; }
        public required string BankName { get; set; }
        public required string IBAN { get; set; }
        public required string CurrencyCode { get; set; }
        public bool IsMain { get; set; } = false;
    }
}
