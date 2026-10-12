using System.ComponentModel.DataAnnotations;

namespace metalurgicaMVC.ViewModels;

public class FiltrosFoto : Paginado
{
    [Range(0, long.MaxValue, ErrorMessage = "La ID del trabajo es incorrecta")]
    public long TrabajoId { get; set; }
}