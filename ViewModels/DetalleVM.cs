using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.Models;

namespace metalurgicaMVC.ViewModels;

public class DetalleVM
{
    [Range(0, long.MaxValue, ErrorMessage = "La ID es incorrecta")]
    public long Id { get; set; }

    [Required(ErrorMessage = "El TrabajoId es requerido")]
    [Range(0, long.MaxValue, ErrorMessage = "El TrabajoId es incorrecto")]
    public long TrabajoId { get; set; }

    [Required(ErrorMessage = "El TipoOperacionId es requerido")]
    [Range(1, int.MaxValue, ErrorMessage = "El TipoOperacionId debe ser una cantidad positiva")]
    public int TipoOperacionId { get; set; }
    public TipoOperacionVM? TipoOperacion { get; set; }

    [Required(ErrorMessage = "El MaterialId es requerido")]
    [Range(1, int.MaxValue, ErrorMessage = "El MaterialId debe ser una cantidad positiva")]
    public int MaterialId { get; set; }
    public MaterialVM? Meterial { get; set; }

    [Range(0.001, 999999.99, ErrorMessage = "El largo debe ser una cantidad positiva")]
    public decimal? Largo { get; set; }

    [Range(0.001, 999999.99, ErrorMessage = "El ancho debe ser una cantidad positiva")]
    public decimal? Ancho { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser positiva")]
    public int Cantidad { get; set; }

    [Range(0.01, 9999.99, ErrorMessage = "Los kg plegados deben ser una cantidad positiva")]
    public decimal? KgPlegados { get; set; }

    [Range(0.001, 99999999.99, ErrorMessage = "El precio del material debe ser una cantidad positiva")]
    public decimal? PrecioMaterial { get; set; }

    [Range(0.001, 99999999.99, ErrorMessage = "El costo fijo del tipo de eoperacion debe ser una cantidad positiva")]
    public decimal? PrecioTipoOperacion { get; set; }
    
    public static DetalleVM From(Detalle vm)
    {
        DetalleVM detalle = new()
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
            detalle.Meterial = MaterialVM.From(vm.Meterial);
        if (vm.TipoOperacion != null)
            detalle.TipoOperacion = TipoOperacionVM.From(vm.TipoOperacion);

        return detalle;
    }

    public static List<DetalleVM> FromList(List<Detalle> detalles)
    {
        var viewModels = new List<DetalleVM>();
        foreach (var d in detalles)
        {
            viewModels.Add(From(d));
        }
        return viewModels;
    }
}