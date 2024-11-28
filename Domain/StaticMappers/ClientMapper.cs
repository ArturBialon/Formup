using Domain.DTO;
using Infrastructure.Models;

namespace Domain.StaticMappers
{
    public static class ClientMapper
    {
        public static ClientDTO MapClientToClientDto(Client client)
        {
            return new ClientDTO
            {
                Name = client.Name,
                Coutry = client.Coutry,
                Zip = client.Zip,
                Street = client.Street,
                Tax = client.Tax,
                Credit = client.Credit
            };
        }
    }
}
