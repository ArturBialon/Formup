using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces.Repository
{
    public interface ICaseDbRepository
    {
        public Task<ICollection<CaseListResponseDTO>> GetCases();
        public Task<CaseResponseDTO> GetCaseById(int id);
        public Task<CaseResponseDTO> AddCase(CaseRequestDTO caseDTO);
        public Task<CaseResponseDTO> EditCase(int idCase, CaseRequestDTO editedCase);
        public Task<CaseResponseDTO> DeleteCaseById(int id);
    }
}
