using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.Models;

namespace metalurgicaMVC.ViewModels;

public class MaterialVM
{
    [Range(0, long.MaxValue, ErrorMessage = "La ID es incorrecta")]
    [Display(Name = "Código")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El tipo de material es Requerido")]
    [AllowedValues(["CHAPA", "CAÑO", "PINTURA"], ErrorMessage = "Tipo de material incorrecto")]
    [Display(Name = "Tipo de Material")]
    public string? Tipo { get; set; }

    [Required(ErrorMessage = "La descripción es Requerida")]
    [MaxLength(255, ErrorMessage = "Supera el máximo de 255 caracteres")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El precio es Requerido")]
    [Range(0.01, 99999999.99, ErrorMessage = "El precio debe ser una cantidad positiva")]
    public decimal Precio { get; set; }

    [Range(0.01, 999999.99, ErrorMessage = "El precio por kg debe ser una cantidad positiva")]
    [Display(Name = "Precio por kg")]
    public decimal? PrecioXkg { get; set; }

    [Range(0.001, 9999.999, ErrorMessage = "El peso por kg debe ser una cantidad positiva")]
    [Display(Name = "Peso por m^2")]
    public decimal? PesoXm2 { get; set; }

    public static MaterialVM From(Material m)
    {
        return new()
        {
            Id = m.Id,
            Tipo = m.Tipo,
            Descripcion = m.Descripcion,
            Precio = m.Precio,
            PrecioXkg = m.PrecioXkg,
            PesoXm2 = m.PesoXm2
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