using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Interfaces;

public interface ITipoOperacionRepository : IRepository<TipoOperacion, int, FiltrosTipoOperacion>
{
    public Task<bool> UpdateCostoFijoAsync(TipoOperacion tipoOperacion);
}