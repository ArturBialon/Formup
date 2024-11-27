using Domain.CustomExceptions;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.Repository;
using Domain.StaticMappers;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Repository.Implementations
{
    public class CostDbRepository : ICostDbRepository
    {
        private readonly FormupContext _context;
        public CostDbRepository(FormupContext compContext)
        {
            _context = compContext;
        }

        public async Task<bool> AddCost(CostRequestDTO cost)
        {
            _context.Costs.Add(CostMapper.MapCostRequestToBase(cost));

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException();
        }

        public async Task<bool> DeleteCost(Cost costFromDb)
        {
            _context.Costs.Remove(costFromDb);

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException();
        }

        public async Task<bool> EditCost(CostRequestDTO cost, Cost costFromDb)
        {
            costFromDb.Amount = cost.Amount;
            costFromDb.ServiceProvidersId = cost.ServiceProvidersId;
            costFromDb.CasesId = cost.CasesId;
            costFromDb.Name = cost.Name;
            costFromDb.Tax = cost.Tax;

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException();
        }

        public async Task<ICollection<CostResponseDTO>> GetCostsAttachedToCase(int caseId)
        {
            var costList = await _context.Costs
                .Where(x => x.CasesId == caseId)
                .Select(x => CostMapper.MapCostBaseToResponse(x))
                .ToListAsync();

            return costList;
        }

        public async Task<Cost> GetCostById(int costId)
        {
            var cost = await _context.Costs
                .Where(x => x.Id == costId)
                .SingleOrDefaultAsync();

            return cost ?? throw new GetEntityException();
        }
    }
}
