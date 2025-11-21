using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public required string Token { get; set; }
        public UserDto? User { get; set; }
    }
}
