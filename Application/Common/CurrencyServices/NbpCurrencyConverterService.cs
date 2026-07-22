using Application.Common.Results;
using System.Net;
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
            DateTime serviceDate,
            CancellationToken ct)
        {
            targetCurrency = targetCurrency.ToUpper().Trim();
            DateTime targetDate = serviceDate.Date.AddDays(-1);

            var ratesResult = await GetRatesTable(targetDate, ct);
            if (!ratesResult.IsSuccess)
            {
                return AppResult<CurrencyConversionResult>.Failure(ratesResult.ErrorCode, ratesResult.ErrorData);
            }

            var ratesMap = ratesResult.Value!;

            if (!ratesMap.ContainsKey(targetCurrency))
            {
                return AppResult<CurrencyConversionResult>.Failure(
                    "CURRENCY.VALIDATION.TARGET_CURRENCY_NOT_SUPPORTED",
                    new { UnsupportedCurrency = targetCurrency }
                );
            }

            foreach (var item in items)
            {
                if (!ratesMap.ContainsKey(item.Currency.ToUpper().Trim()))
                {
                    return AppResult<CurrencyConversionResult>.Failure(
                        "CURRENCY.VALIDATION.ITEM_CURRENCY_NOT_SUPPORTED",
                        new { item.WorCaseItemId, UnsupportedCurrency = item.Currency }
                    );
                }
            }

            var result = new CurrencyConversionResult { TargetCurrency = targetCurrency };
            decimal totalTargetAmount = 0m;
            decimal totalAmountInPln = 0m;

            foreach (var item in items)
            {
                var itemCurrency = item.Currency.ToUpper().Trim();
                decimal finalItemAmount;
                decimal appliedRate;

                if (item.Currency == "PLN")
                {
                    totalAmountInPln += item.Amount;
                }
                else
                {
                    decimal rateFrom = ratesMap[itemCurrency];
                    totalAmountInPln += decimal.Round(item.Amount * rateFrom, 2);
                }

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
                    ItemId = item.WorCaseItemId,
                    OriginalAmount = item.Amount,
                    OriginalCurrency = item.Currency,
                    ConvertedAmount = finalItemAmount,
                    ExchangeRateUsed = decimal.Round(appliedRate, 4)
                });
            }

            result.TotalTargetAmount = totalTargetAmount;
            result.TotalAmountInPln = totalAmountInPln;
            return AppResult<CurrencyConversionResult>.Success(result);
        }

        public async Task<AppResult<decimal>> ConvertToTargetCurrency(
            decimal amount,
            string sourceCurrency,
            string targetCurrency,
            DateTime serviceDate,
            CancellationToken ct)
        {
            sourceCurrency = sourceCurrency.ToUpper().Trim();
            targetCurrency = targetCurrency.ToUpper().Trim();
            DateTime targetDate = serviceDate.Date.AddDays(-1);

            if (sourceCurrency == targetCurrency)
            {
                return AppResult<decimal>.Success(decimal.Round(amount, 2));
            }

            var ratesResult = await GetRatesTable(targetDate, ct);
            if (!ratesResult.IsSuccess)
            {
                return AppResult<decimal>.Failure(ratesResult.ErrorCode, ratesResult.ErrorData);
            }

            var ratesMap = ratesResult.Value!;

            if (!ratesMap.TryGetValue(sourceCurrency, out decimal rateFrom))
            {
                return AppResult<decimal>.Failure("CURRENCY.VALIDATION.SOURCE_CURRENCY_NOT_SUPPORTED", new { UnsupportedCurrency = sourceCurrency });
            }

            if (!ratesMap.TryGetValue(targetCurrency, out decimal rateTo))
            {
                return AppResult<decimal>.Failure("CURRENCY.VALIDATION.TARGET_CURRENCY_NOT_SUPPORTED", new { UnsupportedCurrency = targetCurrency });
            }

            decimal appliedRate = rateFrom / rateTo;
            var finalAmount = decimal.Round(amount * appliedRate, 2);

            return AppResult<decimal>.Success(finalAmount);
        }

        private async Task<AppResult<Dictionary<string, decimal>>> GetRatesTable(DateTime targetDate, CancellationToken ct)
        {
            List<NbpTableA>? nbpTables = null;

            for (int i = 0; i < 7; i++)
            {
                try
                {
                    string formattedDate = targetDate.ToString("yyyy-MM-dd");
                    string url = $"https://api.nbp.pl/api/exchangerates/tables/a/{formattedDate}/?format=json";

                    var response = await _httpClient.GetAsync(url, ct);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        nbpTables = await response.Content.ReadFromJsonAsync<List<NbpTableA>>(cancellationToken: ct);
                        if (nbpTables != null && nbpTables.Count > 0)
                        {
                            break;
                        }
                    }

                    if (response.StatusCode != HttpStatusCode.NotFound)
                    {
                        return AppResult<Dictionary<string, decimal>>.Failure("CURRENCY.API_NBP_UNAVAILABLE");
                    }
                }
                catch
                {
                    if (i == 6)
                        return AppResult<Dictionary<string, decimal>>.Failure("CURRENCY.API_NBP_UNAVAILABLE");
                }

                targetDate = targetDate.AddDays(-1);
            }

            var tableA = nbpTables?.FirstOrDefault();
            if (tableA?.Rates == null)
                return AppResult<Dictionary<string, decimal>>.Failure("CURRENCY.TABLE_A_EMPTY");

            var ratesMap = tableA.Rates.ToDictionary(x => x.Code.ToUpper().Trim(), x => x.Mid);
            ratesMap.TryAdd("PLN", 1.0m);

            return AppResult<Dictionary<string, decimal>>.Success(ratesMap);
        }
    }
}