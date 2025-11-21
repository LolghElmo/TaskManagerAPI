using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
