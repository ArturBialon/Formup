using Infrastructure.Models;
using System;

namespace Application.DTOs.Response
{
    public class UserResponseDTO
    {
        public Forwarder.EntityId Id { get; set; }
        public required string UserName { get; set; }
        public required ResponseTokenDTO Token { get; set; }
    }
}
