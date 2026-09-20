using System.ComponentModel.DataAnnotations;

namespace metalurgicaMVC.ViewModels;

public class Paginado
{
    [Range(1, 50, ErrorMessage = "El limite de resultados por búsqueda es de 50")]
    public int Limit { get; set; } = 10;

    [Range(1, int.MaxValue, ErrorMessage = "La página no puede ser negativa o 0")]
    public int Pagina { get; set; } = 1;

    public int Offset() => (Pagina - 1) * Limit;
}