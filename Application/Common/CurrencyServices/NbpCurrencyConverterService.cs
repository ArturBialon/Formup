using Application.Common.Results;
using System.Net.Http.Json;

namespace Application.Common.CurrencyServices
{
    public class NbpTableA { public List<NbpRateA>? Rates { get; set; } }
    public class NbpRateA { public string Code { get; set; } = null!; public decimal Mid { get; set; } }

    public class NbpCurrencyConverterService(HttpClient httpClient) : ICurrencyConverterService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<AppResult<CurrencyConversionResult>> ConvertCurrenciesAsync(
            List<CurrencyConversionInput> items,
            string targetCurrency,
            decimal? manualExchangeRate,
            CancellationToken ct)
        {
            targetCurrency = targetCurrency.ToUpper();

            List<NbpTableA>? nbpTables;
            try
            {
                nbpTables = await _httpClient.GetFromJsonAsync<List<NbpTableA>>(
                    "http://api.nbp.pl/api/exchangerates/tables/a/?format=json", ct);
            }
            catch
            {
                return AppResult<CurrencyConversionResult>.Failure("CURRENCY.API_NBP_UNAVAILABLE");
            }

            var tableA = nbpTables?.FirstOrDefault();
            if (tableA?.Rates == null) return AppResult<CurrencyConversionResult>.Failure("CURRENCY.TABLE_A_EMPTY");

            var ratesMap = tableA.Rates.ToDictionary(x => x.Code.ToUpper(), x => x.Mid);
            ratesMap.Add("PLN", 1.0m);

            if (!ratesMap.ContainsKey(targetCurrency))
            {
                return AppResult<CurrencyConversionResult>.Failure(
                    "CURRENCY.VALIDATION.TARGET_CURRENCY_NOT_SUPPORTED",
                    new { UnsupportedCurrency = targetCurrency }
                );
            }

            foreach (var item in items)
            {
                if (!ratesMap.ContainsKey(item.Currency.ToUpper()))
                {
                    return AppResult<CurrencyConversionResult>.Failure(
                        "CURRENCY.VALIDATION.ITEM_CURRENCY_NOT_SUPPORTED",
                        new { item.ItemId, UnsupportedCurrency = item.Currency }
                    );
                }
            }

            var result = new CurrencyConversionResult { TargetCurrency = targetCurrency };
            decimal totalTargetAmount = 0m;

            foreach (var item in items)
            {
                var itemCurrency = item.Currency.ToUpper();
                decimal finalItemAmount;
                decimal appliedRate;

                if (itemCurrency == targetCurrency)
                {
                    finalItemAmount = item.Amount;
                    appliedRate = 1.0m;
                }
                else if (targetCurrency == "PLN" && manualExchangeRate.HasValue)
                {
                    appliedRate = manualExchangeRate.Value;
                    finalItemAmount = decimal.Round(item.Amount * appliedRate, 2);
                }
                else
                {
                    decimal rateFrom = ratesMap[itemCurrency];
                    decimal rateTo = ratesMap[targetCurrency];

                    appliedRate = rateFrom / rateTo;
                    finalItemAmount = decimal.Round(item.Amount * appliedRate, 2);
                }

                totalTargetAmount += finalItemAmount;

                result.Details.Add(new ConvertedItemDetail
                {
                    ItemId = item.ItemId,
                    OriginalAmount = item.Amount,
                    OriginalCurrency = item.Currency,
                    ConvertedAmount = finalItemAmount,
                    ExchangeRateUsed = decimal.Round(appliedRate, 4)
                });
            }

            result.TotalTargetAmount = totalTargetAmount;
            return AppResult<CurrencyConversionResult>.Success(result);
        }
    }
}
