using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
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
