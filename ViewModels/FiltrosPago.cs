using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.Utils;

namespace metalurgicaMVC.ViewModels;

public class FiltrosPago : Paginado
{
    [Range(0, long.MaxValue, ErrorMessage = "ID del trabajo incorrecta")]
    public int TrabajoId { get; set; }

    [FechaMenorOIgualQueHoy]
    [Display(Name = "Fecha de Pago")]
    public DateTime? Fecha { get; set; }

    [FechaMenorOIgualQueHoy]
    [Display(Name = "Desde")]
    public DateTime? Desde { get; set; }

    [FechaMayorQue(nameof(Desde), false, ErrorMessage = "Debe ser mayor a Hasta")]
    [Display(Name = "Hasta")]
    public DateTime? Hasta { get; set; }

    [Display(Name = "Cliente")]
    [MaxLength(30, ErrorMessage = "La búsqueda acepta 30 caracteres máximo")]
    public string? Cliente { get; set; }
}