namespace Application.Common.Jobs
{
    public interface IWorkCaseCurrencyJobService
    {
        Task ProcessWorkCasesBatchAsync(CancellationToken ct = default);
        Task ProcessClientCreditsBatchAsync(CancellationToken ct = default);
    }
}
