using Domain.DTO.Request;
using Domain.DTO.Response;
using Infrastructure.Models;

namespace Domain.Interfaces.Repository
{
    public interface ICostDbRepository
    {
        Task<ICollection<CostResponseDTO>> GetCostsAttachedToCase(int caseId);
        Task<Cost> GetCostById(int costId);
        Task<bool> AddCost(CostRequestDTO cost);
        Task<bool> EditCost(CostRequestDTO cost, Cost costFromDb);
        Task<bool> DeleteCost(Cost costId);
    }
}
