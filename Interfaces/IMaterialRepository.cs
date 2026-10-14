using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Interfaces;

public interface IMaterialRepository : IRepository<Material, int, FiltrosMaterial>
{
    public Task<bool> UpdatePrecioAsync(Material material);
}