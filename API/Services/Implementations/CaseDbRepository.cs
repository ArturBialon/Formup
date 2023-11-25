using Infrastructure.Context;
using Infrastructure.DTO.Request;
using Infrastructure.DTO.Response;
using Infrastructure.Enum;
using Infrastructure.Models;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.Implementations
{
    public class CaseDbRepository : ICaseDbRepository
    {

        private readonly FormupContext _context;

        public CaseDbRepository(FormupContext context)
        {
            _context = context;
        }
        public async Task<CommonEnum> AddCase(CaseRequestDTO caseDTO)
        {
            CommonEnum message = CommonEnum.UNKNOWN_ERROR;

            await _context.AddAsync(new Case
            {
                Name = caseDTO.Name,
                Amount = caseDTO.Amount,
                ForwardersId = caseDTO.ForwarderId,
                Relation = caseDTO.Relation
            });

            if (await _context.SaveChangesAsync() > 0)
                message = CommonEnum.SUCCESSFULLY_ADDED;
            //"case successfully added";
            else
                message = CommonEnum.CANNOT_SAVE;
            //"could not save changes";

            return message;
        }

        public async Task<CommonEnum> DeleteCaseById(int id)
        {
            bool flag;
            CommonEnum message = CommonEnum.UNKNOWN_ERROR;

            var caseFromDb = await _context.Cases.Where(x => x.Id == id).SingleOrDefaultAsync();

            if (caseFromDb != null)
            {
                _context.Remove(caseFromDb);
                flag = true;
                message = CommonEnum.SUCCESSFULLY_FOUND;
                //"case found";
            }
            else
            {
                flag = false;
                message = CommonEnum.CANNOT_FIND;
                //"could not find case with given ID";
            }
            if (flag)
            {
                flag = await _context.SaveChangesAsync() > 0;

                if (flag)
                    message = CommonEnum.SUCCESSFULLY_REMOVED;
                //"case successfully removed";
                else
                    message = CommonEnum.CANNOT_SAVE;
                //" could not save changes";
            }


            return message;
        }

        public async Task<CommonEnum> EditCase(int idCase, CaseRequestDTO editedCase)
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

            var @case = await _context.Cases
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

            return @case;
        }

        public async Task<ICollection<CaseListResponseDTO>> GetCases()
        {
            var cases = await _context.Cases
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

            return cases;
        }
    }
}
