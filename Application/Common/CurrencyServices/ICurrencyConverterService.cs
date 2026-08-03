using Application.Common.Results;
using Domain.Models;

namespace Application.Common.CurrencyServices
{
    public interface ICurrencyConverterService
    {
        Task<AppResult<decimal>> ConvertToTargetCurrency(
            decimal totalTargetAmount,
            string currencyCode,
            string targetCurrency,
            DateTime serviceDate,
            CancellationToken ct);

        Task<AppResult<CurrencyConversionResult>> ConvertCurrenciesAsync(
            List<CurrencyConversionInput> items,
            string targetCurrencyCode,
            decimal? manualExchangeRate,
            DateTime serviceDate,
            CancellationToken ct);

        Task<AppResult<int>> RecalculateWorkCaseAmountsBatchAsync(
            List<WorkCase> workCases,
            DateTime serviceDate,
            CancellationToken ct = default);

        Task<AppResult<int>> RecalculateClientCreditsBatchAsync(
            List<Client> clients,
            DateTime serviceDate,
            CancellationToken ct = default);
    }
}
