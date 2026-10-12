using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Interfaces;

public interface IFotoRepository : IRepository<Foto, long, FiltrosFoto>
{
    public Task<int> BulkCreateAsync(List<Foto> fotos);
}