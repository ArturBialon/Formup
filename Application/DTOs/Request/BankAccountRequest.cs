namespace Application.DTOs.Request
{
    public class BankAccountRequest
    {
        public Guid? Id { get; set; }
        public string BankName { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public bool IsMain { get; set; }
    }
}