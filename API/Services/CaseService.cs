using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WorkCaseService : IWorkCaseService
    {
        private readonly IWorkCaseDbRepository _dbRepository;

        public WorkCaseService(IWorkCaseDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ICollection<WorkCaseListResponseDTO>> GetWorkCases()
        {
            return await _dbRepository.GetAllWorkCases();
        }

        public async Task<WorkCaseResponseDTO> GetWorkCaseById(Guid id)
        {
            return await _dbRepository.GetWorkCaseById(id);
        }

        public async Task<bool> CreateNewWorkCase(WorkCaseRequestDTO caseDTO)
        {
            return await _dbRepository.AddWorkCase(caseDTO);
        }

        public async Task<bool> EditWorkCase(WorkCaseRequestDTO editedWorkCase)
        {
            var caseFromDb = await _dbRepository.GetRawWorkCaseById(editedWorkCase.Id);
            return await _dbRepository.EditWorkCase(editedWorkCase, caseFromDb);
        }

        public async Task<bool> DeleteWorkCaseById(Guid id)
        {
            var caseFromDb = await _dbRepository.GetRawWorkCaseById(id);
            return await _dbRepository.DeleteWorkCase(caseFromDb);
        }
    }
}
