using Application.Common.CurrencyServices;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Common.Jobs
{
    public class WorkCaseCurrencyJobService(
        FormupContext context,
        ICurrencyConverterService currencyConverter,
        ILogger<WorkCaseCurrencyJobService> logger) : IWorkCaseCurrencyJobService
    {
        private readonly FormupContext _context = context;
        private readonly ICurrencyConverterService _currencyConverter = currencyConverter;
        private readonly ILogger<WorkCaseCurrencyJobService> _logger = logger;

        private const int BATCH_SIZE = 512;

        public async Task ProcessWorkCasesBatchAsync(CancellationToken ct)
        {
            _logger.LogInformation("Retrieving work cases for currency recalculation...");

            var totalCount = await _context.WorkCases
                .Where(wc => !wc.IsCompleted && !wc.IsAbandoned && wc.CurrencyCode != "PLN")
                .CountAsync(ct);

            if (totalCount == 0)
            {
                _logger.LogInformation("No open cases in foreign currencies.");
                return;
            }

            _logger.LogInformation("Found {Total} cases. Processing in batches of {BatchSize}...", totalCount, BATCH_SIZE);

            int processedCount = 0;

            while (processedCount < totalCount)
            {
                var batch = await _context.WorkCases
                    .Where(wc => !wc.IsCompleted && !wc.IsAbandoned && wc.CurrencyCode != "PLN")
                    .OrderBy(wc => wc.Id)
                    .Skip(processedCount)
                    .Take(BATCH_SIZE)
                    .ToListAsync(ct);

                if (batch.Count == 0) break;

                var result = await _currencyConverter.RecalculateWorkCaseAmountsBatchAsync(batch, DateTime.UtcNow, ct);

                if (result.IsSuccess)
                {
                    await _context.SaveChangesAsync(ct);
                    processedCount += batch.Count;
                    _logger.LogInformation("Recalculated and saved: {Processed}/{Total}", processedCount, totalCount);
                }
                else
                {
                    _logger.LogError("Error occurred while recalculating currency rates in WorkCase batch: {ErrorCode}", result.ErrorCode);
                    break;
                }
            }
        }

        public async Task ProcessClientCreditsBatchAsync(CancellationToken ct)
        {
            _logger.LogInformation("Retrieving clients (Client) for credit limit recalculation...");

            var totalCount = await _context.Clients
                .Where(c => c.IsActive && c.CurrencyCode != "PLN" && c.Credit > 0)
                .CountAsync(ct);

            if (totalCount == 0)
            {
                _logger.LogInformation("No active clients with credit limits in foreign currencies.");
                return;
            }

            _logger.LogInformation("Found {Total} clients. Processing in batches of {BatchSize}...", totalCount, BATCH_SIZE);

            int processedCount = 0;

            while (processedCount < totalCount)
            {
                var batch = await _context.Clients
                    .Where(c => c.IsActive && c.CurrencyCode != "PLN" && c.Credit > 0)
                    .OrderBy(c => c.Id)
                    .Skip(processedCount)
                    .Take(BATCH_SIZE)
                    .ToListAsync(ct);

                if (batch.Count == 0) break;

                var result = await _currencyConverter.RecalculateClientCreditsBatchAsync(batch, DateTime.UtcNow, ct);

                if (result.IsSuccess)
                {
                    await _context.SaveChangesAsync(ct);
                    processedCount += batch.Count;
                    _logger.LogInformation("Recalculated and saved: {Processed}/{Total}", processedCount, totalCount);
                }
                else
                {
                    _logger.LogError("Error occurred while recalculating currency rates in Client Credit batch: {ErrorCode}", result.ErrorCode);
                    break;
                }
            }
        }
    }
}