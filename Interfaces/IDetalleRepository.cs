using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Interfaces;

public interface IDetalleRepository : IRepository<Detalle, long, FiltrosDetalle>
{
    
}