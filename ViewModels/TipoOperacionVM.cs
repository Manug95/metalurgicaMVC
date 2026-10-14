using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.Models;

namespace metalurgicaMVC.ViewModels;

public class TipoOperacionVM
{
    [Range(0, int.MaxValue, ErrorMessage = "El ID no puede ser negativo")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(45, ErrorMessage = "Supera el máximo de 45 caracteres")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "El costo fijo es requerido")]
    [Range(0.001, double.MaxValue, ErrorMessage = "El costo fijo debe ser una cantidad positiva")]
    public decimal? CostoFijo { get; set; }

    public static TipoOperacionVM From(TipoOperacion to)
    {
        return new()
        {
            Id = to.Id,
            Nombre = to.Nombre,
            CostoFijo = to.CostoFijo
        };
    }

    public static List<TipoOperacionVM> FromList(List<TipoOperacion> tiposOperacion)
    {
        var viewModels = new List<TipoOperacionVM>();
        foreach (var to in tiposOperacion)
        {
            viewModels.Add(From(to));
        }
        return viewModels;
    }
}