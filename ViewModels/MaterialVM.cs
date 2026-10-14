using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.Models;

namespace metalurgicaMVC.ViewModels;

public class MaterialVM
{
    [Range(0, long.MaxValue, ErrorMessage = "La ID es incorrecta")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El tipo de material es Requerido")]
    [AllowedValues(["SIN_TIPO", "CHAPA", "CAÑO", "PINTURA"])]
    public string? Tipo { get; set; }

    [MaxLength(255, ErrorMessage = "Supera el máximo de 255 caracteres")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El precio es Requerido")]
    [Range(0.1, double.MaxValue, ErrorMessage = "El precio debe ser una cantidad positiva")]
    public decimal Precio { get; set; }

    [Range(0.1, double.MaxValue, ErrorMessage = "El precio por kg debe ser una cantidad positiva")]
    public decimal? PrecioXkg { get; set; }

    [Range(0.001, double.MaxValue, ErrorMessage = "El peso por kg debe ser una cantidad positiva")]
    public decimal? PesoXm2 { get; set; }

    [MaxLength(50, ErrorMessage = "Supera el máximo de 50 caracteres")]
    public string? Color { get; set; }

    [Range(0.1, double.MaxValue, ErrorMessage = "Los litros deben ser una cantidad positiva")]
    public decimal? Litros { get; set; }

    public static MaterialVM From(Material m)
    {
        return new()
        {
            Id = m.Id,
            Tipo = m.Tipo,
            Descripcion = m.Descripcion,
            Precio = m.Precio,
            PrecioXkg = m.PrecioXkg,
            PesoXm2 = m.PesoXm2,
            Color = m.Color,
            Litros = m.Litros
        };
    }

    public static List<MaterialVM> FromList(List<Material> materiales)
    {
        var viewModels = new List<MaterialVM>();
        foreach (var m in materiales)
        {
            viewModels.Add(From(m));
        }
        return viewModels;
    }
}