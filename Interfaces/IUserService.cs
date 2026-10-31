using System.Security.Claims;

namespace metalurgicaMVC.Interfaces;

public interface IUserService
{
    public int? GetUserId(ClaimsPrincipal claimsPrincipal);
    public string HashearPassword(string password, IConfiguration config);
}