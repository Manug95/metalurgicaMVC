namespace metalurgicaMVC.ViewModels;

public class FiltrosPago : Paginado
{
    public int TrabajoId { get; set; }
    public DateTime? Fecha { get; set; }
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
}