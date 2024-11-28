using Domain.DTO.Response;
using Infrastructure.Models;

namespace Domain.StaticMappers
{
    public static class ForwarderMapper
    {
        public static ForwarderResponseDTO MapForwarderToForwarderResponseDTO(Forwarder forwarderFromDb)
        {
            return new ForwarderResponseDTO
            {
                Login = forwarderFromDb.Name,
                Prefix = forwarderFromDb.Prefix,
                Surname = forwarderFromDb.Surname,
                Id = forwarderFromDb.Id,
            };
        }
    }
}
