using Application.Common.Results;

namespace Application.Common.CurrencyServices
{
    public interface ICurrencyConverterService
    {
        Task<AppResult<CurrencyConversionResult>> ConvertCurrenciesAsync(
            List<CurrencyConversionInput> items,
            string targetCurrency,
            decimal? manualExchangeRate,
            CancellationToken ct);
    }
}
