using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace metalurgicaMVC.Repositories;

public class EmpleadoRepository(DBContext context) : BaseRepository(context), IEmpleadoRepository
{
    public async Task<int> CountAsync(FiltrosEmpleado filters)
    {
        try
        {
            IQueryable<Empleado> empleados = _context.Empleados;

            if (!string.IsNullOrWhiteSpace(filters.Busqueda))
            {
                return await empleados
                    .CountAsync(e => EF.Functions.Like(e.Nombre, $"{filters.Busqueda}%") || EF.Functions.Like(e.Apellido, $"{filters.Busqueda}%"));
            }
            else
                return await empleados.CountAsync();
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

    public async Task<int> CreateAsync(Empleado empleado)
    {
        try
        {
            empleado.Id = 0;
            _context.Empleados.Add(empleado);
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
            throw new AppException("No se pudo guardar el empleado", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Empleado? empleado = await GetByIdAsync(id);

        try
        {
            empleado?.Activo = false;
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
            throw new AppException("No se pudo eliminar el empleado", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al eliminar el empleado", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<Empleado?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Empleados.FindAsync(id);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public Task<List<Empleado>> ListAsync(int limit, int offset)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Empleado>> ListAsync(FiltrosEmpleado filters)
    {
        try
        {
            IQueryable<Empleado> empleados = _context.Empleados;

            if (!string.IsNullOrWhiteSpace(filters.Busqueda))
            {
                empleados = empleados
                    .Where(c => EF.Functions.Like(c.Nombre, $"{filters.Busqueda}%") || EF.Functions.Like(c.Apellido, $"{filters.Busqueda}%"));
            }

            // empleados = empleados.Where(c => c.FechaFin == null);

            if (filters.Limit > 0 && filters.Offset() >= 0)
                empleados = empleados.Skip(filters.Offset()).Take(filters.Limit);

            return await empleados.ToListAsync();
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

    public async Task<bool> UpdateAsync(Empleado empleado)
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
            throw new AppException("No se pudo actualizar el empleado", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al actualizar el empleado", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }
}