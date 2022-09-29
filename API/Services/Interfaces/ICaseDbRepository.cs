using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.Response;
using API.DTO.Request;
using API.Enum;

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
