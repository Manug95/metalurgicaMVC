using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace metalurgicaMVC.Repositories;

public class TipoOperacionRepository(DBContext dBContext) : BaseRepository(dBContext), ITipoOperacionRepository
{
    public Task<int> CountAsync(FiltrosTipoOperacion filters)
    {
        throw new NotImplementedException();
    }

    public Task<int> CreateAsync(TipoOperacion entidad)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
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
            IQueryable<TipoOperacion> tiposOperaciones = _context.TipoOperaciones;

            if (filters.Limit > 0 && filters.Offset() >= 0)
                tiposOperaciones = tiposOperaciones.Skip(filters.Offset()).Take(filters.Limit);

            return await tiposOperaciones.ToListAsync();
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

    public Task<bool> UpdateAsync(TipoOperacion entidad)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateCostoFijoAsync(TipoOperacion tipoOperacion)
    {
        try
        {
            _context.Attach(tipoOperacion);
            _context.Entry(tipoOperacion).Property(nameof(TipoOperacion.CostoFijo)).IsModified = true;
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