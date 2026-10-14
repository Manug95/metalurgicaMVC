using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Models;

public class Detalle
{
    [Key]
    public long Id { get; set; }
    public long TrabajoId { get; set; }
    public int TipoOperacionId { get; set; }
    [ForeignKey("TipoOperacionId")]
    public TipoOperacion? TipoOperacion { get; set; }
    public int MaterialId { get; set; }
    [ForeignKey("MaterialId")]
    public Material? Meterial { get; set; }
    public decimal? Largo { get; set; }
    public decimal? Ancho { get; set; }
    public int Cantidad { get; set; }
    public decimal? KgPlegados { get; set; }
    public decimal? PrecioMaterial { get; set; }
    public decimal? PrecioTipoOperacion { get; set; }
    
    public static Detalle From(DetalleVM vm)
    {
        Detalle detalle = new()
        {
            Id = vm.Id,
            TrabajoId = vm.TrabajoId,
            TipoOperacionId = vm.TipoOperacionId,
            MaterialId = vm.MaterialId,
            Largo = vm.Largo,
            Ancho = vm.Ancho,
            Cantidad = vm.Cantidad,
            KgPlegados = vm.KgPlegados,
            PrecioMaterial = vm.PrecioMaterial,
            PrecioTipoOperacion = vm.PrecioTipoOperacion
        };

        if (vm.Meterial != null)
            detalle.Meterial = Material.From(vm.Meterial);
        if (vm.TipoOperacion != null)
            detalle.TipoOperacion = TipoOperacion.From(vm.TipoOperacion);

        return detalle;
    }

    public static List<Detalle> FromList(List<DetalleVM> viewModels)
    {
        var detalles = new List<Detalle>();
        foreach (var vm in viewModels)
        {
            detalles.Add(From(vm));
        }
        return detalles;
    }
}