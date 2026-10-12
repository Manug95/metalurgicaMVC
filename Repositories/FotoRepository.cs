using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
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

    public Task<long> CreateAsync(Foto entidad)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<Foto?> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Foto>> ListAsync(int limit, int offset)
    {
        throw new NotImplementedException();
    }

    public Task<List<Foto>> ListAsync(FiltrosFoto filters)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Foto entidad)
    {
        throw new NotImplementedException();
    }
}