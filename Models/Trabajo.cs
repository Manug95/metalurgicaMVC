using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Models;

public enum EstadoTrabajo
{
    PRESUPUESTO,
    ACEPTADO,
    RECHAZADO
}

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

    public string? Detalle { get; set; }

    public string? Observacion { get; set; }

    public EstadoTrabajo? Estado { get; set; }

    [NotMapped]
    public int DiasTrabajados { get; set; }

    [NotMapped]
    public int Horas { get; set; }

    [Required]
    public int ClienteId { get; set; }

    [ForeignKey("ClienteId")]
    public Cliente? Cliente { get; set; }

    public bool Anulado { get; set; } = false;

    public List<Foto>? Fotos { get; set; }

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
            Detalle = vm.Detalle,
            Observacion = vm.Observacion,
            DiasTrabajados = vm.DiasTrabajados,
            Estado = vm.Estado
        };

        if (vm.Fotos != null)
            trabajo.Fotos = Foto.FromList(vm.Fotos);

        // if (dto.Cliente != null)
        //     trabajo.Cliente = Cliente.Parse(dto.Cliente);

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
        if (vm.Detalle != Detalle)
            Detalle = vm.Detalle;
        if (vm.Observacion != Observacion)
            Observacion = vm.Observacion;
        if (vm.DiasTrabajados != DiasTrabajados)
            DiasTrabajados = vm.DiasTrabajados;
    }
}