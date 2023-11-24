using API.DTO.Request;
using API.DTO.Response;
using API.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface ICostDbRepository
    {
        Task<ICollection<CostResponseDTO>> GetCosts(int caseId);
        Task<CommonEnum> AddCost(CostRequestDTO cost);
        Task<CommonEnum> EditCost(int id, CostRequestDTO cost);
        Task<CommonEnum> DeleteCost(int id);
    }
}
