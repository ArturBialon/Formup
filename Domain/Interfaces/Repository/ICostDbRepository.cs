using Domain.DTO.Request;
using Domain.DTO.Response;
using Infrastructure.Models;

namespace Domain.Interfaces.Repository
{
    public interface ICostDbRepository
    {
        Task<ICollection<CostResponseDTO>> GetCostsAttachedToWorkCase(WorkCase.EntityId caseId);
        Task<ICollection<CostResponseDTO>> GetCostsAssingedToForwarder(Forwarder.EntityId forwarderId);
        Task<Cost> GetCostById(Cost.EntityId costId);
        Task<bool> AddCost(CostRequestDTO cost);
        Task<bool> EditCost(CostRequestDTO cost, Cost costFromDb);
        Task<bool> DeleteCost(Cost costFromDb);
    }
}
