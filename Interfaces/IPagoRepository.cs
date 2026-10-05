using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Interfaces;

public interface IPagoRepository : IRepository<Pago, long, FiltrosPago>
{
    
}