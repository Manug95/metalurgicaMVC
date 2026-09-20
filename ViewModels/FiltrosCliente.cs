using System.ComponentModel.DataAnnotations;

namespace metalurgicaMVC.ViewModels;

public class FiltrosCliente : Paginado
{
    [MaxLength(60, ErrorMessage = "La búsqueda acepta 60 caracteres máximo")]
    public string? Busqueda { get; set; }
}