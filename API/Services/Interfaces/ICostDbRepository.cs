using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ICostDbRepository
    {
        Task<ICollection<CostResponseDTO>> GetCosts(int caseId);
        Task<CommonEnum> AddCost(CostRequestDTO cost);
        Task<CommonEnum> EditCost(int id, CostRequestDTO cost);
        Task<CommonEnum> DeleteCost(int id);
    }
}
