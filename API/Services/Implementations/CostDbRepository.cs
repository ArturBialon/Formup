using Infrastructure.Context;
using Infrastructure.DTO.Request;
using Infrastructure.DTO.Response;
using Infrastructure.Enum;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.Implementations
{
    public class CostDbRepository : ICostDbRepository
    {
        private readonly FormupContext _context;
        public CostDbRepository(FormupContext compContext)
        {
            _context = compContext;
        }

        public async Task<CommonEnum> AddCost(CostRequestDTO cost)
        {
            throw new NotImplementedException();
        }

        public async Task<CommonEnum> EditCost(int id, CostRequestDTO cost)
        {
            throw new NotImplementedException();
        }

        public async Task<CommonEnum> DeleteCost(int id)
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
