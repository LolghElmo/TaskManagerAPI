using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Application.DTOs.Auth
{
    public class RegisterRequestDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
