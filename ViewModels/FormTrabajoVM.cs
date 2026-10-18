using metalurgicaMVC.Models;

namespace metalurgicaMVC.ViewModels;

public class FormTrabajoVM(TrabajoVM trabajo, List<Material> materiales, List<TipoOperacion> tipoOperaciones)
{
    public TrabajoVM Trabajo { get; set; } = trabajo;
    public List<Material> Materiales { get; set; } = materiales;
    public int CantidadPaginasMateriales { get; set; }
    public List<TipoOperacion> TipoOperaciones { get; set; } = tipoOperaciones;
}