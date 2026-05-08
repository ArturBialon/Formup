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
    public class WorkCaseDbRepository(FormupContext context, IForwarderDbRepository forwarderDbRepository) : IWorkCaseDbRepository
    {
        
        public async Task<bool> AddWorkCase(WorkCaseRequestDTO caseDTO)
        {
            var forwarder = await forwarderDbRepository.GetForwarderById(caseDTO.ForwarderId);

            await context.AddAsync(new WorkCase
            {
                Name = caseDTO.Name,
                Amount = caseDTO.Amount,
                Forwarders = forwarder,
                Relation = caseDTO.Relation
            });

            if (await context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException($"Could not save entity + {caseDTO.Name}");
        }

        public async Task<bool> DeleteWorkCase(WorkCase caseFromDb)
        {
            context.Remove(caseFromDb);
            if (await context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException($"No changes made, Id: {caseFromDb.Id}");
        }

        public async Task<bool> EditWorkCase(WorkCaseRequestDTO editedWorkCase, WorkCase caseFromDb)
        {
            var forwarder = await forwarderDbRepository.GetForwarderById(editedWorkCase.ForwarderId);

            caseFromDb.Name = editedWorkCase.Name;
            caseFromDb.Relation = editedWorkCase.Relation;
            caseFromDb.Amount = editedWorkCase.Amount;
            caseFromDb.Forwarders = forwarder;

            if (await context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new SavingException($"No changes were made, Id: {editedWorkCase.Id}");
        }

        public async Task<WorkCaseResponseDTO> GetWorkCaseById(WorkCase.EntityId id)
        {
            decimal totalCost = 0;
            decimal totalSales = 0;

            var costsList = await context.Costs
                .Where(x => x.WorkCase.Id == id)
                .ToListAsync();

            var salesList = await context.Invoices
                .Where(x => x.WorkCase.Id == id)
                .ToListAsync();

            totalCost = costsList.Sum(x => x.Amount);
            totalSales = salesList.Sum(x => x.Amount);

            var response = await context.WorkCases
            .Where(x => x.Id == id)
            .Select(x => new WorkCaseResponseDTO
            {
                Id = x.Id,
                Name = x.Name,
                Amount = x.Amount,
                Relation = x.Relation,
                ClientName = x.Invoices.FirstOrDefault().Client.Name,
                ForwarderName = x.Forwarders.Name,
                NumberOfInvoices = x.Invoices.Count,
                TotalCosts = totalCost,
                TotalSales = totalSales
            })
            .SingleOrDefaultAsync();

            return response ?? throw new GetEntityException();
        }

        public async Task<ICollection<WorkCaseListResponseDTO>> GetAllWorkCases()
        {
            var response = new List<WorkCaseListResponseDTO>();

            response = await context.WorkCases
            .Select(x => new WorkCaseListResponseDTO
            {
                Name = x.Name,
                ClientName = x.Invoices.FirstOrDefault().Client.Name,
                ForwarderName = x.Forwarders.Name,
                NumberOfInvoices = x.Invoices.Count,
                TotalCosts = x.Costs.ToList().Sum(x => x.Amount),
                TotalSales = x.Invoices.ToList().Sum(x => x.Amount)
            })
            .ToListAsync();

            return response;
        }

        public async Task<WorkCase> GetRawWorkCaseById(WorkCase.EntityId id)
        {
            var response = await context.WorkCases.Where(x => x.Id == id).SingleOrDefaultAsync();
            return response ?? throw new GetEntityException();
        }

        public async Task<IEnumerable<WorkCase>> GetForwardersWorkCases(Forwarder.EntityId forwarderId)
        {
            var response = await context.WorkCases.Where(x => x.Forwarders.Id == forwarderId).ToListAsync();
            return response;
        }
    }
}
