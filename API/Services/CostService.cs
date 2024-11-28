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

        public async Task<bool> AddCost(CostRequestDTO cost)
        {
            var isAdded = await _dbRepository.AddCost(cost);
            return isAdded;
        }

        public async Task<bool> DeleteCost(int costId)
        {
            var costFromDb = await _dbRepository.GetCostById(costId);
            var isDeleted = await _dbRepository.DeleteCost(costFromDb);
            return isDeleted;
        }

        public async Task<bool> EditCost(CostRequestDTO cost)
        {
            var costFromDb = await _dbRepository.GetCostById(cost.Id);
            var isEdited = await _dbRepository.EditCost(cost, costFromDb);
            return isEdited;
        }

        public async Task<ICollection<CostResponseDTO>> GetCostsAttachedToCase(int caseId)
        {
            return await _dbRepository.GetCostsAttachedToCase(caseId);
        }
    }
}
