namespace metalurgicaMVC.ViewModels;

public class FiltrosTrabajo : Paginado
{
    public string? Cliente { get; set; }
    public bool ReturnUpdated { get; set; } = false;

    public DateTime? FechaSolicitado { get; set; }
    public DateTime? FechaIniciado { get; set; }
    public DateTime? FechaTerminado { get; set; }
    public DateTime? FechaRetirado { get; set; }
}