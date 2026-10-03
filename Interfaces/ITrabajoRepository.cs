using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Interfaces;

public interface ITrabajoRepository : IRepository<Trabajo, long, FiltrosTrabajo>
{
    
}