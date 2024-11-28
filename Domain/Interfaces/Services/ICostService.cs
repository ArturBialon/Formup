using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces.Services
{
    public interface ICostService
    {
        Task<ICollection<CostResponseDTO>> GetCostsAttachedToCase(int caseId);
        Task<bool> AddCost(CostRequestDTO cost);
        Task<bool> EditCost(CostRequestDTO cost);
        Task<bool> DeleteCost(int costId);
    }
}
