using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces.Services
{
    public interface ICostService
    {
        Task<ICollection<CostResponseDTO>> GetCostsAttachedToWorkCase(Guid caseId);
        Task<bool> AddCost(CostRequestDTO cost);
        Task<bool> EditCost(CostRequestDTO cost);
        Task<bool> DeleteCost(Guid costId);
    }
}
