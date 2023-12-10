using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces.Repository
{
    public interface ICostDbRepository
    {
        Task<ICollection<CostResponseDTO>> GetCosts(int caseId);
        Task<CostResponseDTO> AddCost(CostRequestDTO cost);
        Task<CostResponseDTO> EditCost(int id, CostRequestDTO cost);
        Task<CostResponseDTO> DeleteCost(int id);
    }
}
