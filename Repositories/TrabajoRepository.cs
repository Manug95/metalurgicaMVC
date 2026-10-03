using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace metalurgicaMVC.Repositories;

public class TrabajoRepository(DBContext dbContext) : BaseRepository(dbContext), ITrabajoRepository
{
    public async Task<long> CountAsync(FiltrosTrabajo filters)
    {
        try
        {
            IQueryable<Trabajo> trabajos = _context.Trabajos;

            // if (!string.IsNullOrWhiteSpace(filters.Busqueda))
            // {
            //     return await empleados
            //         .CountAsync(e => EF.Functions.Like(e.Nombre, $"{filters.Busqueda}%") || EF.Functions.Like(e.Apellido, $"{filters.Busqueda}%"));
            // }
            // else
                return await trabajos.CountAsync();
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

    public async Task<long> CreateAsync(Trabajo trabajo)
    {
        try
        {
            trabajo.Id = 0;
            _context.Trabajos.Add(trabajo);
            return await _context.SaveChangesAsync();
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo guardar el trabajo", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        Trabajo? trabajo = await GetByIdAsync(id);

        try
        {
            trabajo?.Anulado = true;
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
            throw new AppException("No se pudo eliminar el trabajo", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al eliminar el trabajo", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<Trabajo?> GetByIdAsync(long id)
    {
        try
        {
            return await _context.Trabajos.FindAsync(id);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public Task<List<Trabajo>> ListAsync(int limit, int offset)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Trabajo>> ListAsync(FiltrosTrabajo filters)
    {
        try
        {
            IQueryable<Trabajo> trabajos = _context.Trabajos.Where(t => !t.Anulado);

            // if (!string.IsNullOrWhiteSpace(filters.Busqueda))
            // {
            //     empleados = empleados
            //         .Where(c => EF.Functions.Like(c.Nombre, $"{filters.Busqueda}%") || EF.Functions.Like(c.Apellido, $"{filters.Busqueda}%"));
            // }

            // empleados = empleados.Where(c => c.FechaFin == null);

            if (filters.Limit > 0 && filters.Offset() >= 0)
                trabajos = trabajos.Skip(filters.Offset()).Take(filters.Limit);

            return await trabajos.ToListAsync();
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

    public async Task<bool> UpdateAsync(Trabajo trabajo)
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
            throw new AppException("No se pudo actualizar el trabajo", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al actualizar el trabajo", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }
}