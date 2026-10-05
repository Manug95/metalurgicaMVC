using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels.UsuarioViewModels;

namespace metalurgicaMVC.ViewModels;

public class PagoVM
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Falta el trabajo asociado")]
    public long TrabajoId { get; set; }

    [Display(Name = "Fecha de Cobro")]
    public DateTime? Fecha { get; set; } = DateTime.Now.ToUniversalTime().AddHours(-3);

    [Required(ErrorMessage = "Valor requerido")]
    [Range(1, double.MaxValue, ErrorMessage = "Debe ser positivo")]
    [Display(Name = "Monto")]
    public decimal? Monto { get; set; }

    [Required(ErrorMessage = "Valor requerido")]
    [AllowedValues(["EFECTIVO", "TRANSFERENCIA", "CHEQUE", "CHEQUE ELECTRONICO", "QR"], ErrorMessage = "Valor inorrecto")]
    [Display(Name = "Forma de Pago")]
    public string? FormaPago { get; set; }

    public bool Anulado { get; set; }

    [Display(Name = "Fecha Anulado")]
    public DateTime? FechaAnulacion { get; set; }

    public UsuarioVM? Cobrador { get; set; }

    public UsuarioVM? Anulador { get; set; }

    public TrabajoVM? Trabajo { get; set; }

    public static PagoVM From(Pago p)
    {
        PagoVM vm = new()
        {
            Id = p.Id,
            TrabajoId = p.TrabajoId,
            Fecha = p.Fecha,
            Monto = p.Monto,
            FormaPago = p.FormaPago,
            Anulado = p.Anulado,
            FechaAnulacion = p.FechaAnulacion
        };

        if (p.Trabajo != null)
            vm.Trabajo = TrabajoVM.From(p.Trabajo);
        if (p.Cobrador != null)
            vm.Cobrador = UsuarioVM.From(p.Cobrador);
        if (p.Anulador != null)
            vm.Anulador = UsuarioVM.From(p.Anulador);

        return vm;
    }

    public static List<PagoVM> FromList(List<Pago> pagos)
    {
        List<PagoVM> viewModels = [];
        foreach (var p in pagos)
        {
            viewModels.Add(From(p));
        }
        return viewModels;
    }
}