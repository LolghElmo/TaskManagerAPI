using IdentityService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        public Task<(bool Succeeded, string Token, string Error)> LoginAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Succeeded, string Error)> RegisterAsync(string email, string password, string firstName, string lastName)
        {
            throw new NotImplementedException();
        }
    }
}
