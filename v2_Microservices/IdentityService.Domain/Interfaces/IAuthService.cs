using Common.Models;
using IdentityService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Domain.Interfaces
{
    public interface IAuthService
    {
        // We are using Identifier to allow either username or email for login 
        Task<Result<(User User, string Token)>> LoginAsync(string identifier, string password);
        Task<Result<User>> RegisterAsync(string email, string username, string password, string firstName, string lastName);
    }
}
