using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace metalurgicaMVC.Repositories;

public class FotoRepository(DBContext dBContext) : BaseRepository(dBContext), IFotoRepository
{
    public async Task<int> BulkCreateAsync(List<Foto> fotos)
    {
        try
        {
            await _context.AddRangeAsync(fotos);
            return await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new AppException("", ex);
        }
    }

    public Task<long> CountAsync(FiltrosFoto filters)
    {
        throw new NotImplementedException();
    }

    public async Task<long> CreateAsync(Foto foto)
    {
        try
        {
            foto.Id = 0;
            _context.Fotos.Add(foto);
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
            throw new AppException("No se pudo guardar la foto", ex);
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
            var entryFoto = _context.Fotos.Local.FindEntry(nameof(Foto.Id), id);
            if (entryFoto != null)
                _context.Fotos.Remove(entryFoto.Entity);
            else
                _context.Fotos.Remove(new Foto() { Id = id });
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

    public async Task<Foto?> GetByIdAsync(long id)
    {
        try
        {
            return await _context.Fotos.FindAsync(id);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public Task<List<Foto>> ListAsync(int limit, int offset)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Foto>> ListAsync(FiltrosFoto filters)
    {
        try
        {
            IQueryable<Foto> fotos = _context.Fotos;

            if (filters.TrabajoId > 0)
                fotos = fotos.Where(f => f.TrabajoId == filters.TrabajoId);

            if (filters.Limit > 0 && filters.Offset() >= 0)
                fotos = fotos.Skip(filters.Offset()).Take(filters.Limit);

            return await fotos.ToListAsync();
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

    public Task<bool> UpdateAsync(Foto entidad)
    {
        throw new NotImplementedException();
    }
}