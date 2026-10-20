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
    public Material? Material { get; set; }
    public decimal? Largo { get; set; }
    public decimal? Ancho { get; set; }
    public int Cantidad { get; set; }
    public decimal? KgPlegados { get; set; }
    public decimal? PrecioMaterial { get; set; }
    public decimal? PrecioTipoOperacion { get; set; }
    [NotMapped]
    public bool Borrado { get; set; } = false;
    
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

        if (vm.Material != null)
            detalle.Material = Material.From(vm.Material);
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

    public void UpdateFrom(DetalleVM vm)
    {
        if (TrabajoId != vm.TrabajoId)
            TrabajoId = vm.TrabajoId;
        if (MaterialId != vm.MaterialId)
            MaterialId = vm.MaterialId;
        if (TipoOperacionId != vm.TipoOperacionId)
            TipoOperacionId = vm.TipoOperacionId;
        if (Largo != vm.Largo)
            Largo = vm.Largo;
        if (Ancho != vm.Ancho)
            Ancho = vm.Ancho;
        if (Cantidad != vm.Cantidad)
            Cantidad = vm.Cantidad;
        if (KgPlegados != vm.KgPlegados)
            KgPlegados = vm.KgPlegados;
        if (PrecioMaterial != vm.PrecioMaterial)
            PrecioMaterial = vm.PrecioMaterial;
        if (PrecioTipoOperacion != vm.PrecioTipoOperacion)
            PrecioTipoOperacion = vm.PrecioTipoOperacion;
    }
}