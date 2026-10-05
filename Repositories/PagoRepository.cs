using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace metalurgicaMVC.Repositories;

public class PagoRepository(DBContext dBContext) : BaseRepository(dBContext), IPagoRepository
{
    public async Task<long> CountAsync(FiltrosPago filters)
    {
        try
        {
            IQueryable<Pago> pagos = _context.Pagos;

            if (filters.TrabajoId > 0)
                pagos = pagos.Include(p => p.Trabajo).Where(p => p.TrabajoId == filters.TrabajoId);
            else if (filters.Cliente != null)
            {
                pagos = pagos
                    .Include(p => p.Trabajo)
                        .ThenInclude(t => t!.Cliente)
                    .Where(p => EF.Functions.Like(p.Trabajo!.Cliente!.Apellido, $"{filters.Cliente}%") 
                        || EF.Functions.Like(p.Trabajo!.Cliente!.Nombre, $"{filters.Cliente}%")
                    );
            }

            if (filters.Fecha.HasValue)
                pagos = pagos.Where(p => p.Fecha == filters.Fecha);
            else if (filters.Desde.HasValue && filters.Hasta.HasValue)
                pagos = pagos.Where(p => p.Fecha >= filters.Desde && p.Fecha <= filters.Hasta);
            
            return await pagos.CountAsync();
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

    public async Task<long> CreateAsync(Pago pago)
    {
        try
        {
            pago.Id = 0;
            _context.Pagos.Add(pago);
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
            throw new AppException("No se pudo guardar el pago", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        Pago? pago = await GetByIdAsync(id);

        try
        {
            pago?.Anulado = true;
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
            throw new AppException("No se pudo eliminar el pago", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al eliminar el pago", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<Pago?> GetByIdAsync(long id)
    {
        try
        {
            return await _context.Pagos.FindAsync(id);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public Task<List<Pago>> ListAsync(int limit, int offset)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Pago>> ListAsync(FiltrosPago filters)
    {
        try
        {
            IQueryable<Pago> pagos = _context.Pagos;

            if (filters.TrabajoId > 0)
                pagos = pagos.Include(p => p.Trabajo).Where(p => p.TrabajoId == filters.TrabajoId);
            else if (filters.Cliente != null)
            {
                pagos = pagos
                    .Include(p => p.Trabajo)
                        .ThenInclude(t => t!.Cliente)
                    .Where(p => EF.Functions.Like(p.Trabajo!.Cliente!.Apellido, $"{filters.Cliente}%") 
                        || EF.Functions.Like(p.Trabajo!.Cliente!.Nombre, $"{filters.Cliente}%")
                    );
            }

            if (filters.Fecha.HasValue)
                pagos = pagos.Where(p => p.Fecha == filters.Fecha);
            else if (filters.Desde.HasValue && filters.Hasta.HasValue)
                pagos = pagos.Where(p => p.Fecha >= filters.Desde && p.Fecha <= filters.Hasta);

            if (filters.Limit > 0 && filters.Offset() >= 0)
                pagos = pagos.Skip(filters.Offset()).Take(filters.Limit);

            return await pagos.ToListAsync();
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

    public async Task<bool> UpdateAsync(Pago pago)
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
            throw new AppException("No se pudo actualizar el pago", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al actualizar el pago", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }
}