using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Application.DTOs.Auth
{
    public class UserDto
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
