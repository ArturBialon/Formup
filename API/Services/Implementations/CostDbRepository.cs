using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interfaces;
using API.Enum;
using API.DTO.Request;
using API.DTO.Response;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implementations
{
    public class CostDbRepository : ICostDbRepository
    {
        private readonly FWD_CompContext _context;
        public CostDbRepository(FWD_CompContext compContext)
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
