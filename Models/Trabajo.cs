using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Models;

public class Trabajo
{
    [Key]
    public long Id { get; set; }

    public DateTime? FechaSolicitado { get; set; }

    public DateTime? FechaIniciado { get; set; }

    public DateTime? FechaFinalizado { get; set; }

    public DateTime? FechaRetirado { get; set; }

    public decimal? Importe { get; set; }

    public string? Descripcion { get; set; }

    // public string? Observacion { get; set; }

    public string? Estado { get; set; }

    public int ClienteId { get; set; }

    [ForeignKey("ClienteId")]
    public Cliente? Cliente { get; set; }

    public bool Anulado { get; set; } = false;

    public int? AnuladorId { get; set; }

    [ForeignKey("AnuladorId")]
    public Usuario? Anulador { get; set; }

    public int RegistradorId { get; set; }

    [ForeignKey("RegistradorId")]
    public Usuario? Registrador { get; set; }

    public List<Foto>? Fotos { get; set; }

    public List<Detalle> Detalles { get; set; } = [];

    public static Trabajo From(TrabajoVM vm)
    {
        Trabajo trabajo = new()
        {
            Id = vm.Id,
            ClienteId = vm.ClienteId,
            FechaIniciado = vm.FechaIniciado,
            FechaSolicitado = vm.FechaSolicitado,
            FechaFinalizado = vm.FechaFinalizado,
            Importe = vm.Importe,
            Descripcion = vm.Descripcion,
            // Observacion = vm.Observacion,
            Estado = vm.Estado
        };

        if (vm.Fotos != null)
            trabajo.Fotos = Foto.FromList(vm.Fotos);
        if (vm.Detalles != null)
            trabajo.Detalles = Detalle.FromList(vm.Detalles);
        if (vm.Anulador != null)
            trabajo.Anulador = Usuario.From(vm.Anulador);
        if (vm.Registrador != null)
            trabajo.Registrador = Usuario.From(vm.Registrador);

        if (vm.Cliente != null)
            trabajo.Cliente = Cliente.From(vm.Cliente);

        return trabajo;
    }

    public static List<Trabajo> FromList(List<TrabajoVM> viewModels)
    {
        List<Trabajo> trabajos = [];
        foreach (var dto in viewModels)
        {
            trabajos.Add(From(dto));
        }
        return trabajos;
    }

    public void UpdateFrom(TrabajoVM vm)
    {
        if (vm.FechaSolicitado != FechaSolicitado)
            FechaSolicitado = vm.FechaSolicitado;
        if (vm.FechaIniciado != FechaIniciado)
            FechaIniciado = vm.FechaIniciado;
        if (vm.FechaFinalizado != FechaFinalizado)
            FechaFinalizado = vm.FechaFinalizado;
        if (vm.FechaRetirado != FechaRetirado)
            FechaRetirado = vm.FechaRetirado;
        if (vm.Importe != Importe)
            Importe = vm.Importe;
        if (vm.Estado != Estado)
            Estado = vm.Estado;
        if (vm.Descripcion != Descripcion)
            Descripcion = vm.Descripcion;
        // if (vm.Observacion != Observacion)
        //     Observacion = vm.Observacion;
        if (vm.Detalles != null)
        {
            foreach (var dVM in vm.Detalles)
            {
                if (dVM.Id == 0)
                {
                    Detalles.Add(new Detalle()
                    {
                        TrabajoId = vm.Id,
                        MaterialId = dVM.MaterialId,
                        TipoOperacionId = dVM.TipoOperacionId,
                        Largo = dVM.Largo,
                        Ancho = dVM.Ancho,
                        Cantidad = dVM.Cantidad,
                        KgPlegados = dVM.KgPlegados,
                        PrecioMaterial = dVM.PrecioMaterial,
                        PrecioTipoOperacion = dVM.PrecioTipoOperacion
                    });
                }
                else
                {
                    var detalle = Detalles.FirstOrDefault(d => d.Id == dVM.Id);
                    detalle?.UpdateFrom(dVM);
                }
            }
            var idsDetallesVM = vm.Detalles.Where(d => d.Id > 0).Select(d => d.Id).ToList();
            // var detallesAEliminar = Detalles.Where(d => !idsDetallesVM.Contains(d.Id) && d.Id > 0).ToList();
            // foreach (var d in detallesAEliminar)
            // {
            //     d.Borrado = true;
            // }
            foreach (var d in Detalles)
            {
                if (!idsDetallesVM.Contains(d.Id) && d.Id > 0)
                    d.Borrado = true;
            }
        }

    }
}