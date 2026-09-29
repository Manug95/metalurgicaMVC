using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Interfaces;

public interface IEmpleadoRepository : IRepository<Empleado, int, FiltrosEmpleado>
{
    
}