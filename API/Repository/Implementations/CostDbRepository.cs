using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.Repository;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
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

        public Task<CostResponseDTO> AddCost(CostRequestDTO cost)
        {
            throw new NotImplementedException();
        }

        public Task<CostResponseDTO> DeleteCost(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CostResponseDTO> EditCost(CostRequestDTO cost)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<CostResponseDTO>> GetCosts(int caseId)
        {
            var costList = await _context.Costs
                .Where(x => x.CasesId == caseId)
                .Select(x => new CostResponseDTO
                {
                    AmountNet = x.Amount,
                    AmountBrut = x.Amount * x.Tax,
                    Tax = x.Tax,
                    Name = x.Name
                })
                .ToListAsync();

            return costList;
        }
    }
}
