using Infrastructure.DTO.Request;
using Infrastructure.DTO.Response;
using Infrastructure.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.Interfaces
{
    public interface ICostDbRepository
    {
        Task<ICollection<CostResponseDTO>> GetCosts(int caseId);
        Task<CommonEnum> AddCost(CostRequestDTO cost);
        Task<CommonEnum> EditCost(int id, CostRequestDTO cost);
        Task<CommonEnum> DeleteCost(int id);
    }
}
