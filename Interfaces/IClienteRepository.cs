using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Interfaces;

public interface IClienteRepository : IRepository<Cliente, int, FiltrosCliente>
{
    
}