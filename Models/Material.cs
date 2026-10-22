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
            PesoXm2 = vm.PesoXm2
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

    public void UpdateFrom(MaterialVM vm)
    {
        if (Tipo != vm.Tipo)
            Tipo = vm.Tipo;
        if (Descripcion != vm.Descripcion)
            Descripcion = vm.Descripcion;
        if (Precio != vm.Precio)
            Precio = vm.Precio;
        if (PrecioXkg != vm.PrecioXkg)
            PrecioXkg = vm.PrecioXkg;
        if (PesoXm2 != vm.PesoXm2)
            PesoXm2 = vm.PesoXm2;
    }
}