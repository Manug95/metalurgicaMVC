using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace metalurgicaMVC.Repositories;

public class DetalleRepository(DBContext dBContext) : BaseRepository(dBContext), IDetalleRepository
{
    public async Task<long> CountAsync(FiltrosDetalle filters)
    {
        try
        {
            return await _context.Detalles.CountAsync();
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

    public async Task<long> CreateAsync(Detalle detalle)
    {
        try
        {
            detalle.Id = 0;
            _context.Detalles.Add(detalle);
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
            throw new AppException("No se pudo guardar el detalle", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            _context.Remove(new Detalle() { Id = id });
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
            throw new AppException("No se pudo eliminar el detalle", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al eliminar el detalle", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<Detalle?> GetByIdAsync(long id)
    {
        try
        {
            return await _context.Detalles.FindAsync(id);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public Task<List<Detalle>> ListAsync(int limit, int offset)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Detalle>> ListAsync(FiltrosDetalle filters)
    {
        try
        {
            IQueryable<Detalle> detalles = _context.Detalles;

            if (filters.Limit > 0 && filters.Offset() >= 0)
                detalles = detalles.Skip(filters.Offset()).Take(filters.Limit);

            return await detalles.ToListAsync();
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

    public async Task<bool> UpdateAsync(Detalle detalle)
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
            throw new AppException("No se pudo actualizar el detalle", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al actualizar el detalle", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }
}