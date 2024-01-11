using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CaseService : ICaseService
    {
        private readonly ICaseDbRepository _dbRepository;

        public CaseService(ICaseDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ICollection<CaseListResponseDTO>> GetCases()
        {
            return await _dbRepository.GetAllCases();
        }

        public async Task<CaseResponseDTO> GetCaseById(int id)
        {
            return await _dbRepository.GetCaseById(id);
        }

        public async Task<CaseResponseDTO> CreateNewCase(CaseRequestDTO caseDTO)
        {
            return await _dbRepository.AddCase(caseDTO);
        }

        public async Task<CaseResponseDTO> EditCase(CaseRequestDTO editedCase)
        {
            return await _dbRepository.EditCase(editedCase);
        }

        public async Task<CaseResponseDTO> DeleteCaseById(int id)
        {
            return await _dbRepository.DeleteCaseById(id);
        }
    }
}
