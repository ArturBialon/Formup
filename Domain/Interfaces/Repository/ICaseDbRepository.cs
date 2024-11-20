﻿using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces.Repository
{
    public interface ICaseDbRepository
    {
        public Task<ICollection<CaseListResponseDTO>> GetAllCases();
        public Task<CaseResponseDTO> GetCaseById(int id);
        public Task<CaseResponseDTO> AddCase(CaseRequestDTO caseDTO);
        public Task<CaseResponseDTO> EditCase(CaseRequestDTO editedCase);
        public Task<CaseResponseDTO> DeleteCaseById(int id);
    }
}