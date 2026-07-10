using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace API.Extensions.ServiceCreator
{
    public class RemoveEqualityContractProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            var parameterToRemove = context.OperationDescription.Operation.Parameters
                .FirstOrDefault(p => p.Name.Equals("EqualityContract", StringComparison.OrdinalIgnoreCase));

            if (parameterToRemove != null)
            {
                context.OperationDescription.Operation.Parameters.Remove(parameterToRemove);
            }

            return true;
        }
    }
}