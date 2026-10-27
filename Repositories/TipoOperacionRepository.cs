using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace metalurgicaMVC.Repositories;

public class TipoOperacionRepository(DBContext dBContext) : BaseRepository(dBContext), ITipoOperacionRepository
{
    public async Task<int> CountAsync(FiltrosTipoOperacion filters)
    {
        try
        {
            return await  _context.TipoOperaciones
                .Where(to => !to.Eliminado)
                .CountAsync();
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

    public async Task<int> CreateAsync(TipoOperacion tipoOperacion)
    {
        try
        {
            tipoOperacion.Id = 0; // por si las moscas
            _context.TipoOperaciones.Add(tipoOperacion);
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
            throw new AppException("No se pudo guardar el tipo de operación", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        TipoOperacion tipoOperacion = await GetByIdAsync(id) ?? throw new ClientException("El tipo de operación no existe");
        try
        {
            tipoOperacion.Eliminado = true;
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
            throw new AppException("No se pudo eliminar el tipo de operación", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al eliminar el tipo de operación", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<TipoOperacion?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.TipoOperaciones.FindAsync(id);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public Task<List<TipoOperacion>> ListAsync(int limit, int offset)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TipoOperacion>> ListAsync(FiltrosTipoOperacion filters)
    {
        try
        {
            IQueryable<TipoOperacion> tiposOperaciones = _context.TipoOperaciones.OrderBy(to => to.Nombre);

            if (filters.Limit > 0 && filters.Offset() >= 0)
                tiposOperaciones = tiposOperaciones.Skip(filters.Offset()).Take(filters.Limit);

            return await tiposOperaciones
                .Where(to => !to.Eliminado)
                .ToListAsync();
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

    public async Task<bool> UpdateAsync(TipoOperacion tipoOperacion)
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
            throw new AppException("No se pudo actualizar el tipo de operación", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al actualizar el tipo de operación", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<bool> UpdateCostoFijoAsync(TipoOperacion tipoOperacion)
    {
        try
        {
            // _context.Attach(tipoOperacion);
            // _context.Entry(tipoOperacion).Property(nameof(TipoOperacion.CostoFijo)).IsModified = true;
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
            throw new AppException("No se pudo actualizar el material", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al actualizar el material", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }
}