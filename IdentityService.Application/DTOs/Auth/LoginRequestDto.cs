using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        // Because user can login with either username or email
        public required string Identifier { get; set; }
        public required string Password { get; set; }
    }
}
