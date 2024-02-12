using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;

namespace Application.Services
{
    public class CostService : ICostService
    {
        private readonly ICostDbRepository _dbRepository;

        public CostService(ICostDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
    }
}
