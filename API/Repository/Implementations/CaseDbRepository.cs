using Domain.CustomExceptions;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Helpers;
using Domain.Interfaces.Repository;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Repository.Implementations
{
    public class CaseDbRepository : ICaseDbRepository
    {

        private readonly FormupContext _context;
        private readonly ILogger _logger;

        public CaseDbRepository(FormupContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<CaseResponseDTO> AddCase(CaseRequestDTO caseDTO)
        {
            CaseResponseDTO response = (CaseResponseDTO)ObjectCreationHelper.GenerateObject(typeof(CaseResponseDTO));

            await _context.AddAsync(new Case
            {
                Name = caseDTO.Name,
                Amount = caseDTO.Amount,
                ForwardersId = caseDTO.ForwarderId,
                Relation = caseDTO.Relation
            });

            if (await _context.SaveChangesAsync() > 0)
            {
                return response;
            }
            else
            {
                throw new SavingException($"Could not save entity + {caseDTO.Name}");
            }
        }

        public async Task<CaseResponseDTO> DeleteCaseById(int id)
        {
            CaseResponseDTO response = (CaseResponseDTO)ObjectCreationHelper.GenerateObject(typeof(CaseResponseDTO));

            var caseFromDb = await _context.Cases.Where(x => x.Id == id).SingleOrDefaultAsync() ?? throw new GetEntityException($"Could not find case, Id: {id}");

            _context.Remove(caseFromDb);
            if (await _context.SaveChangesAsync() > 0)
            {
                return response;
            }
            else
            {
                throw new SavingException($"No changes made, Id: {id}");
            }
        }

        public async Task<CaseResponseDTO> EditCase(CaseRequestDTO editedCase)
        {
            CaseResponseDTO response = (CaseResponseDTO)ObjectCreationHelper.GenerateObject(typeof(CaseResponseDTO));

            var caseToEdit = await _context.Cases.Where(x => x.Id == editedCase.Id).SingleOrDefaultAsync() ?? throw new GetEntityException($"Could not find case: {editedCase.Name}");

            _logger.Information($"Case to edit found: {caseToEdit} - {editedCase}");
            caseToEdit.Name = editedCase.Name;
            caseToEdit.Relation = editedCase.Relation;
            caseToEdit.Amount = editedCase.Amount;
            caseToEdit.ForwardersId = editedCase.ForwarderId;

            if (await _context.SaveChangesAsync() > 0)
            {
                return response;
            }
            else
            {
                _logger.Warning($"No changes were made, Id: {editedCase.Id}");
                throw new SavingException($"No changes were made, Id: {editedCase.Id}");
            }
        }

        public async Task<CaseResponseDTO> GetCaseById(int id)
        {
            CaseResponseDTO response = (CaseResponseDTO)ObjectCreationHelper.GenerateObject(typeof(CaseResponseDTO));

            decimal totalCost = 0;
            decimal totalSales = 0;

            var costsList = await _context.Costs
                .Where(x => x.CasesId == id)
                .ToListAsync();

            var salesList = await _context.Invoices
                .Where(x => x.CasesId == id)
                .ToListAsync();

            if (costsList != null && salesList != null)
            {
                foreach (Cost cost in costsList)
                {
                    totalCost += cost.Amount;
                }
                foreach (Invoice invoice in salesList)
                {
                    totalSales += invoice.Amount;
                }
            }

            response = await _context.Cases
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

            return response;
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
    }
}
