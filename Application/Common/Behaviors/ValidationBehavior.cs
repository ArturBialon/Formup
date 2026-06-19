using Application.Common.Results;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            if (!_validators.Any()) return await next(ct);

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, ct)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                var errorsDictionary = failures
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorCode).ToList()
                    );

                var responseType = typeof(TResponse);
                if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(AppResult<>))
                {
                    var validationFailureMethod = responseType.GetMethod("ValidationFailure", BindingFlags.Public | BindingFlags.Static);

                    if (validationFailureMethod != null)
                    {
                        var appResultFailure = validationFailureMethod.Invoke(null, [errorsDictionary]);
                        return (TResponse)appResultFailure!;
                    }
                }
            }

            return await next(ct);
        }
    }
}