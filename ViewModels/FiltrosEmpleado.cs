using System.ComponentModel.DataAnnotations;

namespace metalurgicaMVC.ViewModels;

public class FiltrosEmpleado : Paginado
{
    [MaxLength(30, ErrorMessage = "La búsqueda acepta 30 caracteres máximo")]
    public string? Busqueda { get; set; }

    // public bool Activo { get; set; } = true;
}