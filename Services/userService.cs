using System.Security.Claims;
using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace metalurgicaMVC.Services;

public class UserService : IUserService
{
    public int? GetUserId(ClaimsPrincipal claimsPrincipal)
    {
        Claim? claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "id");
        string? idStr = claim?.Value;
        if (int.TryParse(idStr, out int id))
            return id;
        else
            return null;
    }

    public string HashearPassword(string password, IConfiguration config)
    {
        if (password == null)
            throw new ClientException("No se ingresó una contraseña");
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"] ?? "el mejor condimento del asado"),
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 1000,
            numBytesRequested: 256 / 8))
        ;
    }
}