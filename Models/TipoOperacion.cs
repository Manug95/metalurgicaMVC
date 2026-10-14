using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Models;

[Table("tipo_operaciones")]
public class TipoOperacion
{
    [Key]
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public decimal? CostoFijo { get; set; }
    public bool Eliminado { get; set; } = false;

    public static TipoOperacion From(TipoOperacionVM vm)
    {
        return new()
        {
            Id = vm.Id,
            Nombre = vm.Nombre,
            CostoFijo = vm.CostoFijo
        };
    }

    public static List<TipoOperacion> FromList(List<TipoOperacionVM> viewModels)
    {
        var tiposOperacion = new List<TipoOperacion>();
        foreach (var vm in viewModels)
        {
            tiposOperacion.Add(From(vm));
        }
        return tiposOperacion;
    }
}