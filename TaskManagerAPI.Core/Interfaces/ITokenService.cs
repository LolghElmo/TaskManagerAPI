using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerAPI.Core.Models;

namespace TaskManagerAPI.Core.Interfaces
{
    public interface ITokenService
    {
        // Handles Token Creation
        string CreateToken(ApplicationUser user);
    }
}
