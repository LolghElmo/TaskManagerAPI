using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Succeeded, string Token, string Error)> LoginAsync(string email, string password);
        Task<(bool Succeeded, string Error)> RegisterAsync(string email, string password, string firstName, string lastName);
    }
}
