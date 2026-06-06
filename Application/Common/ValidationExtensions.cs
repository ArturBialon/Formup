using Domain.Constants;
using FluentValidation;
namespace Application.Common
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeValidRelation<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Relation type required.")
                .Must(relation => Relations.All.Contains(relation))
                .WithMessage($"Invalid relation type, only following available: {string.Join(", ", Relations.All)}.");
        }
    }
}