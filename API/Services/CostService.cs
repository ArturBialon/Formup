using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CostService : ICostService
    {
        private readonly ICostDbRepository _dbRepository;

        public CostService(ICostDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<CostResponseDTO> AddCost(CostRequestDTO cost)
        {
            return await _dbRepository.AddCost(cost);
        }

        public async Task<bool> DeleteCost(int costId)
        {
            return await _dbRepository.DeleteCost(costId);
        }

        public async Task<CostResponseDTO> EditCost(CostRequestDTO cost)
        {
            return await _dbRepository.EditCost(cost);
        }

        public async Task<ICollection<CostResponseDTO>> GetCostsAttachedToCase(int caseId)
        {
            return await _dbRepository.GetCostsAttachedToCase(caseId);
        }
    }
}
