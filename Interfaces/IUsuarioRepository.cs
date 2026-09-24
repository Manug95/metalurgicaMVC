using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario, int, FiltrosUsuario>
{
    public Task<Usuario?> GetByUsernameAsync(string username);
    public Task<bool> UpdatePasswordAsync();
    public Task<bool> DeleteAvatarAsync(int id);
    public Task<bool> UpdateAvatarAsync(int id, string avatar);
}