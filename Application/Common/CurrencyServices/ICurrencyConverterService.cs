using Application.Common.Results;

namespace Application.Common.CurrencyServices
{
    public interface ICurrencyConverterService
    {
        Task<AppResult<decimal>> ConvertToTargetCurrency(
            decimal totalTargetAmount,
            string currency,
            string targetCurrency,
            DateTime serviceDate,
            CancellationToken ct);

        Task<AppResult<CurrencyConversionResult>> ConvertCurrenciesAsync(
            List<CurrencyConversionInput> items,
            string targetCurrency,
            decimal? manualExchangeRate,
            DateTime serviceDate,
            CancellationToken ct);
    }
}
