using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces.Services
{
    public interface ICaseService
    {
        public Task<ICollection<CaseListResponseDTO>> GetCases();
        public Task<CaseResponseDTO> GetCaseById(int id);
        public Task<CaseResponseDTO> CreateNewCase(CaseRequestDTO caseDTO);
        public Task<CaseResponseDTO> EditCase(CaseRequestDTO editedCase);
        public Task<CaseResponseDTO> DeleteCaseById(int id);
    }
}
