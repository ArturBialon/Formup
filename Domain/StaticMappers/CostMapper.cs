using Domain.DTO.Request;
using Domain.DTO.Response;
using Infrastructure.Models;

namespace Domain.StaticMappers
{
    public static class CostMapper
    {
        public static CostResponseDTO MapCostRequestToResponse(CostRequestDTO cost)
        {
            return new CostResponseDTO
            {
                Id = cost.Id,
                Name = cost.Name,
                Tax = cost.Tax,
                AmountBrut = decimal.Multiply(cost.Amount, cost.Tax),
                AmountNet = cost.Amount
            };
        }

        public static Cost MapCostRequestToBase(CostRequestDTO cost)
        {
            return new Cost
            {
                Id = cost.Id,
                Name = cost.Name,
                Amount = cost.Amount,
                Tax = cost.Tax,
                CasesId = cost.CasesId,
                ServiceProvidersId = cost.ServiceProvidersId
            };
        }

        public static CostResponseDTO MapCostBaseToResponse(Cost cost)
        {
            return new CostResponseDTO
            {
                Id = cost.Id,
                AmountNet = cost.Amount,
                AmountBrut = decimal.Multiply(cost.Amount, cost.Tax),
                Tax = cost.Tax,
                Name = cost.Name
            };
        }

    }
}
