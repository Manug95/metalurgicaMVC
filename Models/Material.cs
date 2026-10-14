using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Models;

public class Material
{
    [Key]
    public int Id { get; set; }
    public string? Tipo { get; set; }
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public decimal? PrecioXkg { get; set; }
    public decimal? PesoXm2 { get; set; }
    public string? Color { get; set; }
    public decimal? Litros { get; set; }
    public bool Eliminado { get; set; } = false;

    public static Material From(MaterialVM vm)
    {
        return new()
        {
            Id = vm.Id,
            Tipo = vm.Tipo,
            Descripcion = vm.Descripcion,
            Precio = vm.Precio,
            PrecioXkg = vm.PrecioXkg,
            PesoXm2 = vm.PesoXm2,
            Color = vm.Color,
            Litros = vm.Litros
        };
    }

    public static List<Material> FromList(List<MaterialVM> viewModels)
    {
        var materiales = new List<Material>();
        foreach (var vm in viewModels)
        {
            materiales.Add(From(vm));
        }
        return materiales;
    }
}