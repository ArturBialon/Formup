using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces.Services
{
    public interface IWorkCaseService
    {
        public Task<ICollection<WorkCaseListResponseDTO>> GetWorkCases();
        public Task<WorkCaseResponseDTO> GetWorkCaseById(Guid id);
        public Task<bool> CreateNewWorkCase(WorkCaseRequestDTO caseDTO);
        public Task<bool> EditWorkCase(WorkCaseRequestDTO editedWorkCase);
        public Task<bool> DeleteWorkCaseById(Guid id);
    }
}
