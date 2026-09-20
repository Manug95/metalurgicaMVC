namespace metalurgicaMVC.Interfaces;

public interface IRepository<T, ID, F>
{
    public Task<T?> GetByIdAsync(ID id);
    public Task<List<T>> ListAsync(int limit, int offset);
    public Task<List<T>> ListAsync(F filters);
    public Task<ID> CreateAsync(T entidad);
    public Task<bool> UpdateAsync(T entidad);
    public Task<bool> DeleteAsync(ID id);
    public Task<ID> CountAsync(F filters);
}