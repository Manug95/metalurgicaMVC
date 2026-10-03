namespace metalurgicaMVC.ViewModels;

public class FiltrosTrabajo : Paginado
{
    public string? Cliente { get; set; }

    public DateTime? FechaSolicitado { get; set; }
    public DateTime? FechaIniciado { get; set; }
    public DateTime? FechaTerminado { get; set; }
    public DateTime? FechaRetirado { get; set; }
}