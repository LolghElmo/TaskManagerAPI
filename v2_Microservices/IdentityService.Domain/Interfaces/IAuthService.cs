using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<Result<string>> LoginAsync(string email, string password);
        Task<Result<bool>> RegisterAsync(string email, string password, string firstName, string lastName);
    }
}
