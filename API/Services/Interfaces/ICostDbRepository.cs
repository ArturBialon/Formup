using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Enum;
using API.DTO.Request;
using API.DTO.Response;

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
