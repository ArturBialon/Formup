using Domain.DTO;
using Infrastructure.Models;

namespace Domain.StaticMappers
{
    public static class ServiceProviderMapper
    {
        public static ServiceProviderDTO MapServiceProviderToDto(ServiceProvider provider)
        {
            return new ServiceProviderDTO
            {
                Id = provider.Id,
                Coutry = provider.Coutry,
                Tax = provider.Tax,
                Name = provider.Name,
                Street = provider.Street,
                Zip = provider.Zip,
            };
        }
    }
}
