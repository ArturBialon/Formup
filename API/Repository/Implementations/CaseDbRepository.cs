using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.Repository;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
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
            CaseResponseDTO response = null;

            await _context.AddAsync(new Case
            {
                Name = caseDTO.Name,
                Amount = caseDTO.Amount,
                ForwardersId = caseDTO.ForwarderId,
                Relation = caseDTO.Relation
            });

            try
            {
                await _context.SaveChangesAsync();
                response.ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                Log.Error($"Unable to add case {caseDTO?.Name ?? "invalid request"}", ex);
                response.ErrorMessage = $"Unable to add case: {caseDTO?.Name ?? "invalid request"}";
            }

            return response;
        }

        public async Task<CaseResponseDTO> DeleteCaseById(int id)
        {
            CaseResponseDTO response = null;
            var caseFromDb = await _context.Cases.Where(x => x.Id == id).SingleOrDefaultAsync();

            try
            {
                _context.Remove(caseFromDb);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error($"Could not remove case: {caseFromDb?.Name ?? "invalid request"}", ex);
                response.ErrorMessage = $"Could not remove case: {caseFromDb.Name}";
            }

            return response;
        }

        public async Task<CaseResponseDTO> EditCase(int idCase, CaseRequestDTO editedCase)
        {
            bool flag;
            CommonEnum message = CommonEnum.UNKNOWN_ERROR;

            var caseToEdit = await _context.Cases.Where(x => x.Id == idCase).SingleOrDefaultAsync();
            if (caseToEdit != null)
            {
                caseToEdit.Name = editedCase.Name;
                caseToEdit.Relation = editedCase.Relation;
                caseToEdit.Amount = editedCase.Amount;
                caseToEdit.ForwardersId = editedCase.ForwarderId;

                flag = true;
                //"case found";
            }
            else
            {
                flag = false;
                message = CommonEnum.CANNOT_FIND;
                //"could not find case";
            }

            if (flag)
            {
                flag = await _context.SaveChangesAsync() > 0;
                if (flag)
                    message = CommonEnum.CHANGES_SAVED;
                //"case successfully updated";
                else
                    message = CommonEnum.CANNOT_SAVE;
                //" cannot save changes";
            }

            return message;
        }

        public async Task<CaseResponseDTO> GetCaseById(int id)
        {

            decimal totalCost = 0;
            decimal totalSales = 0;

            try
            {
                var costsList = await _context.Costs
                    .Where(x => x.CasesId == id)
                    .ToListAsync();

                var salesList = await _context.Invoices
                    .Where(x => x.CasesId == id)
                    .ToListAsync();

                foreach (Cost cost in costsList)
                {
                    totalCost += cost.Amount;
                }
                foreach (Invoice invoice in salesList)
                {
                    totalSales += invoice.Amount;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            var transportCase = await _context.Cases
                .Where(x => x.Id == id)
                .Select(x => new CaseResponseDTO
                {
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

            return transportCase;
        }

        public async Task<ICollection<CaseListResponseDTO>> GetCases()
        {
            var cases = new List<CaseListResponseDTO>();
            try
            {
                cases = await _context.Cases
                .Select(x => new CaseListResponseDTO
                {
                    Name = x.Name,
                    ClientName = x.Invoices.FirstOrDefault().Clients.Name,
                    ForwarderName = x.Forwarders.Name,
                    NumberOfInvoices = x.Invoices.Count,
                    TotalCosts = x.Costs.ToList().Sum(x => x.Amount),
                    TotalSales = x.Invoices.ToList().Sum(x => x.Amount),
                    ErrorMessage = ""
                })
                .ToListAsync();
            }
            catch (Exception ex)
            {

            }


            return cases;
        }
    }
}
