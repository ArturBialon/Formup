using API.DTO.Request;
using API.DTO.Response;
using API.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface ICaseDbRepository
    {
        public Task<ICollection<CaseListResponseDTO>> GetCases();
        public Task<CaseResponseDTO> GetCaseById(int id);
        public Task<CommonEnum> AddCase(CaseRequestDTO caseDTO);
        public Task<CommonEnum> EditCase(int idCase, CaseRequestDTO editedCase);
        public Task<CommonEnum> DeleteCaseById(int id);
    }
}
