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

        public async Task<bool> CreateNewCase(CaseRequestDTO caseDTO)
        {
            return await _dbRepository.AddCase(caseDTO);
        }

        public async Task<bool> EditCase(CaseRequestDTO editedCase)
        {
            var caseFromDb = await _dbRepository.GetRawCaseById(editedCase.Id);
            return await _dbRepository.EditCase(editedCase, caseFromDb);
        }

        public async Task<bool> DeleteCaseById(int id)
        {
            var caseFromDb = await _dbRepository.GetRawCaseById(id);
            return await _dbRepository.DeleteCase(caseFromDb);
        }
    }
}
