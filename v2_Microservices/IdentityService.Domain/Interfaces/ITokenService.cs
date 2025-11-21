using IdentityService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Domain.Interfaces
{
    public interface ITokenService
    {
        // Generates a JWT token 
        string GenerateToken(User user);
    }
}
