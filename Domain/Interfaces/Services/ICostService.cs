using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces.Services
{
    public interface ICostService
    {
        Task<ICollection<CostResponseDTO>> GetCostsAttachedToCase(int caseId);
        Task<CostResponseDTO> AddCost(CostRequestDTO cost);
        Task<CostResponseDTO> EditCost(CostRequestDTO cost);
        Task<bool> DeleteCost(int costId);
    }
}
