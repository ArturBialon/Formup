using Domain.CustomExceptions;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.Repository;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Repository.Implementations
{
    public class CaseDbRepository : ICaseDbRepository
    {

        private readonly FormupContext _context;

        public CaseDbRepository(FormupContext context)
        {
            _context = context;
        }
        public async Task<bool> AddCase(CaseRequestDTO caseDTO)
        {
            await _context.AddAsync(new Case
            {
                Name = caseDTO.Name,
                Amount = caseDTO.Amount,
                ForwardersId = caseDTO.ForwarderId,
                Relation = caseDTO.Relation
            });

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException($"Could not save entity + {caseDTO.Name}");
        }

        public async Task<bool> DeleteCase(Case caseFromDb)
        {
            _context.Remove(caseFromDb);
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException($"No changes made, Id: {caseFromDb.Id}");
        }

        public async Task<bool> EditCase(CaseRequestDTO editedCase, Case caseFromDb)
        {
            caseFromDb.Name = editedCase.Name;
            caseFromDb.Relation = editedCase.Relation;
            caseFromDb.Amount = editedCase.Amount;
            caseFromDb.ForwardersId = editedCase.ForwarderId;

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException($"No changes were made, Id: {editedCase.Id}");
        }

        public async Task<CaseResponseDTO> GetCaseById(int id)
        {
            decimal totalCost = 0;
            decimal totalSales = 0;

            var costsList = await _context.Costs
                .Where(x => x.CasesId == id)
                .ToListAsync();

            var salesList = await _context.Invoices
                .Where(x => x.CasesId == id)
                .ToListAsync();

            totalCost = costsList.Sum(x => x.Amount);
            totalSales = salesList.Sum(x => x.Amount);

            var response = await _context.Cases
            .Where(x => x.Id == id)
            .Select(x => new CaseResponseDTO
            {
                Id = x.Id,
                Name = x.Name,
                Amount = x.Amount,
                Relation = x.Relation,
                ClientName = x.Invoices.FirstOrDefault().Clients.Name,
                ForwarderName = x.Forwarders.Name,
                NumberOfInvoices = x.Invoices.Count,
                TotalCosts = totalCost,
                TotalSales = totalSales
            })
            .SingleOrDefaultAsync();

            return response ?? throw new GetEntityException();
        }

        public async Task<ICollection<CaseListResponseDTO>> GetAllCases()
        {
            var response = new List<CaseListResponseDTO>();

            response = await _context.Cases
            .Select(x => new CaseListResponseDTO
            {
                Name = x.Name,
                ClientName = x.Invoices.FirstOrDefault().Clients.Name,
                ForwarderName = x.Forwarders.Name,
                NumberOfInvoices = x.Invoices.Count,
                TotalCosts = x.Costs.ToList().Sum(x => x.Amount),
                TotalSales = x.Invoices.ToList().Sum(x => x.Amount)
            })
            .ToListAsync();

            return response;
        }

        public async Task<Case> GetRawCaseById(int id)
        {
            var response = await _context.Cases.Where(x => x.Id == id).SingleOrDefaultAsync();
            return response ?? throw new GetEntityException();
        }
    }
}
