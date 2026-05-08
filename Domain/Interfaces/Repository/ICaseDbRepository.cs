using Domain.DTO.Request;
using Domain.DTO.Response;
using Infrastructure.Models;

namespace Domain.Interfaces.Repository
{
    public interface IWorkCaseDbRepository
    {
        public Task<ICollection<WorkCaseListResponseDTO>> GetAllWorkCases();
        public Task<IEnumerable<WorkCase>> GetForwardersWorkCases(Forwarder.EntityId forwarderId);
        public Task<WorkCaseResponseDTO> GetWorkCaseById(WorkCase.EntityId id);
        public Task<WorkCase> GetRawWorkCaseById(WorkCase.EntityId id);
        public Task<bool> AddWorkCase(WorkCaseRequestDTO caseDTO);
        public Task<bool> EditWorkCase(WorkCaseRequestDTO caseToEdit, WorkCase caseFromDb);
        public Task<bool> DeleteWorkCase(WorkCase caseFromDb);
    }
}
