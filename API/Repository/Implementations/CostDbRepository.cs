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
    public class CostDbRepository(FormupContext context) : ICostDbRepository
    {

        public async Task<bool> AddCost(CostRequestDTO cost)
        {
            context.Costs.Add(CostMapper.MapCostRequestToBase(cost));

            if (await context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException();
        }

        public async Task<bool> DeleteCost(Cost costFromDb)
        {
            context.Costs.Remove(costFromDb);

            if (await context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException();
        }

        public async Task<bool> EditCost(CostRequestDTO cost, Cost costFromDb)
        {
            costFromDb.Amount = cost.Amount;
            costFromDb.Name = cost.Name;
            costFromDb.Tax = cost.Tax;

            if (await context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException();
        }

        public async Task<ICollection<CostResponseDTO>> GetCostsAttachedToWorkCase(WorkCase.EntityId caseId)
        {
            var costList = await context.Costs
                .Where(x => x.WorkCase.Id == caseId)
                .Select(x => CostMapper.MapCostBaseToResponse(x))
                .ToListAsync();

            return costList;
        }

        public async Task<Cost> GetCostById(Cost.EntityId costFromDb)
        {
            var cost = await context.Costs
                .Where(x => x.Id == costFromDb)
                .SingleOrDefaultAsync();

            return cost ?? throw new GetEntityException();
        }

        public async Task<ICollection<CostResponseDTO>> GetCostsAssingedToForwarder(Forwarder.EntityId forwarderId)
        {
            var rawData = await context.Costs
                .Where(x => x.WorkCase.Forwarders.Id == forwarderId)
                .ToListAsync();
            var response = rawData.Select(x => CostMapper.MapCostBaseToResponse(x)).ToList();

            return response;
        }
    }
}
