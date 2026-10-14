using metalurgicaMVC.Models;

namespace metalurgicaMVC.ViewModels;

public class FormTrabajoVM(Trabajo trabajo, List<Material> materiales, List<TipoOperacion> tipoOperaciones)
{
    public Trabajo Trabajo { get; set; } = trabajo;
    public List<Material> Materiales { get; set; } = materiales;
    public List<TipoOperacion> TipoOperaciones { get; set; } = tipoOperaciones;
}