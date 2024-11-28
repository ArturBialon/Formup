using Domain.DTO.Request;
using Domain.DTO.Response;
using Infrastructure.Models;

namespace Domain.Interfaces.Repository
{
    public interface ICaseDbRepository
    {
        public Task<ICollection<CaseListResponseDTO>> GetAllCases();
        public Task<CaseResponseDTO> GetCaseById(int id);
        public Task<Case> GetRawCaseById(int id);
        public Task<bool> AddCase(CaseRequestDTO caseDTO);
        public Task<bool> EditCase(CaseRequestDTO caseToEdit, Case caseFromDb);
        public Task<bool> DeleteCase(Case caseFromDb);
    }
}
