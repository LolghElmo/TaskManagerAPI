using IdenttiyService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdenttiyService.Domain.Interfaces
{
    public interface ITokenService
    {
        // Handles Token Generation
        string GenerateToken(User user);
    }
}
