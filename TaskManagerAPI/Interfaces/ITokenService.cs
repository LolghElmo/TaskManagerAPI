using TaskManagerAPI.Models;

namespace TaskManagerAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}
