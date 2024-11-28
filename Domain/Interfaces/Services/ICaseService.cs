using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces.Services
{
    public interface ICaseService
    {
        public Task<ICollection<CaseListResponseDTO>> GetCases();
        public Task<CaseResponseDTO> GetCaseById(int id);
        public Task<bool> CreateNewCase(CaseRequestDTO caseDTO);
        public Task<bool> EditCase(CaseRequestDTO editedCase);
        public Task<bool> DeleteCaseById(int id);
    }
}
