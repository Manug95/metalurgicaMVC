using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels.UsuarioViewModels;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace metalurgicaMVC.Repositories;

public class UsuarioRepository(DBContext context) : BaseRepository(context), IUsuarioRepository
{
    public async Task<int> CountAsync(FiltrosUsuario filters)
    {
        try
        {
            IQueryable<Usuario> usuarios = _context.Usuarios;

            if (!string.IsNullOrWhiteSpace(filters.Username))
            {
                usuarios = usuarios
                    .Where(u => EF.Functions.Like(u.Username, $"{filters.Username}%"));
            }
            if (!string.IsNullOrWhiteSpace(filters.Rol))
            {
                usuarios = usuarios
                    .Where(u => EF.Functions.Like(u.Rol, $"{filters.Rol}%"));
            }
            if (filters.Estado.HasValue)
            {
                usuarios = filters.Estado.Value == EstadoUsuario.ACTIVO
                    ? usuarios.Where(c => c.Activo)
                    : usuarios.Where(c => !c.Activo)
                ;
            }

            return await usuarios.CountAsync();
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (ArgumentNullException)
        {
            throw new ClientException("No se pasaron valores");
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<int> CreateAsync(Usuario usuario)
    {
        try
        {
            usuario.Id = 0; // por si las moscas
            _context.Usuarios.Add(usuario);
            return await _context.SaveChangesAsync();
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is MySqlException exception)
            {
                if (exception.Number == ERR_UNIQUE)
                {
                    if (exception.Message.Contains("username"))
                        throw new ClientException($"El nombre de usuario '{usuario.Username}' ya existe");
                    if (exception.Message.Contains("avatar"))
                        throw new ClientException($"El avatar ya existe");
                }
            }
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo guardar el usuario", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Usuario? usuario = await GetByIdAsync(id);

        try
        {
            usuario?.Activo = false;
            return await _context.SaveChangesAsync() > 0;
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo eliminar el usuario", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al eliminar el usuario", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<bool> DeleteAvatarAsync(int id)
    {
        Usuario? usuario = await GetByIdAsync(id);

        try
        {
            usuario?.Avatar = null;
            return await _context.SaveChangesAsync() > 0;
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo eliminar el avatar del usuario", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al eliminar el avatar del usuario", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Usuarios.FindAsync(id);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<Usuario?> GetByUsernameAsync(string username)
    {
        try
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public Task<List<Usuario>> ListAsync(int limit, int offset)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Usuario>> ListAsync(FiltrosUsuario filters)
    {
        try
        {
            IQueryable<Usuario> usuarios = _context.Usuarios;

            if (!string.IsNullOrWhiteSpace(filters.Username))
            {
                usuarios = usuarios
                    .Where(u => EF.Functions.Like(u.Username, $"{filters.Username}%"));
            }
            if (!string.IsNullOrWhiteSpace(filters.Rol))
            {
                usuarios = usuarios
                    .Where(u => EF.Functions.Like(u.Rol, $"{filters.Rol}%"));
            }
            if (filters.Estado.HasValue)
            {
                usuarios = filters.Estado.Value == EstadoUsuario.ACTIVO
                    ? usuarios.Where(c => c.Activo)
                    : usuarios.Where(c => !c.Activo)
                ;
            }

            if (filters.Limit > 0 && filters.Offset() >= 0)
                usuarios = usuarios.Skip(filters.Offset()).Take(filters.Limit);

            return await usuarios.ToListAsync();
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (ArgumentNullException)
        {
            throw new ClientException("No se pasaron valores");
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<bool> UpdateAsync(Usuario usuario)
    {
        try
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo actualizar el usuario", ex);
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is MySqlException exception)
            {
                if (exception.Number == ERR_UNIQUE)
                {
                    if (exception.Message.Contains("username"))
                        throw new ClientException($"El nombre de usuario '{usuario.Username}' ya existe");
                    if (exception.Message.Contains("avatar"))
                        throw new ClientException($"El avatar ya existe");
                }
            }
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al actualizar el usuario", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<bool> UpdateAvatarAsync(int id, string avatar)
    {
        Usuario? usuario = await GetByIdAsync(id);

        try
        {
            usuario?.Avatar = avatar;
            return await _context.SaveChangesAsync() > 0;
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo actualizar el avatar", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al actualizar el avatar", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<bool> UpdatePasswordAsync()
    {
        try
        {
            return await _context.SaveChangesAsync() > 0;
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo actualizar la contraseña", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al actualizar la contraseña", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }
}