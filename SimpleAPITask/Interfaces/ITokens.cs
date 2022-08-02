using ProjectApp.Domain;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleAPITask.Interfaces
{
    public interface ITokens
    {
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<Token> GenerateJWTTokensAsync(string email);
    }
}
