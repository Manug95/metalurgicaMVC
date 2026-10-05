using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Models;

public class Pago
{
    [Key]
    public long Id { get; set; }

    public long TrabajoId { get; set; }

    [ForeignKey("TrabajoId")]
    public Trabajo? Trabajo { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal? Monto { get; set; }

    public string? FormaPago { get; set; }

    public bool Anulado { get; set; } = false;

    public DateTime? FechaAnulacion { get; set; }

    public int CobradorId { get; set; }

    [ForeignKey("CobradorId")]
    public Usuario? Cobrador { get; set; }

    public int AnuladorId { get; set; }

    [ForeignKey("AnuladorId")]
    public Usuario? Anulador { get; set; }

    public static Pago From(PagoVM vm)
    {
        Pago pago = new()
        {
            Id = vm.Id,
            TrabajoId = vm.TrabajoId,
            Fecha = vm.Fecha,
            Monto = vm.Monto,
            FormaPago = vm.FormaPago,
            Anulado = vm.Anulado,
            FechaAnulacion = vm.FechaAnulacion
        };

        if (vm.Trabajo != null)
            pago.Trabajo = Trabajo.From(vm.Trabajo);
        if (vm.Cobrador != null)
            pago.Cobrador = Usuario.From(vm.Cobrador);
        if (vm.Anulador != null)
            pago.Anulador = Usuario.From(vm.Anulador);

        return pago;
    }

    public static List<Pago> FromList(List<PagoVM> viewModels)
    {
        List<Pago> pagos = [];
        foreach (var vm in viewModels)
        {
            pagos.Add(From(vm));
        }
        return pagos;
    }

    public void CopyFrom(Pago p)
    {
        Id = p.Id;
        TrabajoId = p.TrabajoId;
        Fecha = p.Fecha;
        Monto = p.Monto;
        FormaPago = p.FormaPago;
        Anulado = p.Anulado;
        FechaAnulacion = p.FechaAnulacion;

        if (p.Trabajo != null)
            Trabajo = p.Trabajo;
        if (p.Cobrador != null)
            Cobrador = p.Cobrador;
        if (p.Anulador != null)
            Anulador = p.Anulador;
    }

    public void CopyFrom(PagoVM vm)
    {
        Id = vm.Id;
        TrabajoId = vm.TrabajoId;
        Fecha = vm.Fecha;
        Monto = vm.Monto;
        FormaPago = vm.FormaPago;
        Anulado = vm.Anulado;
        FechaAnulacion = vm.FechaAnulacion;

        if (vm.Trabajo != null)
            Trabajo = Trabajo.From(vm.Trabajo);
        if (vm.Cobrador != null)
            Cobrador = Usuario.From(vm.Cobrador);
        if (vm.Anulador != null)
            Anulador = Usuario.From(vm.Anulador);
    }

    public void UpdateFrom(PagoVM vm)
    {
        if (vm.TrabajoId != TrabajoId)
            TrabajoId = vm.TrabajoId;
        if (vm.Fecha != Fecha)
            Fecha = vm.Fecha;
        if (vm.Monto != Monto)
            Monto = vm.Monto;
        if (vm.FormaPago != FormaPago)
            FormaPago = vm.FormaPago;
        if (vm.Anulado != Anulado)
            Anulado = vm.Anulado;
        if (vm.FechaAnulacion != FechaAnulacion)
            FechaAnulacion = vm.FechaAnulacion;
    }
}