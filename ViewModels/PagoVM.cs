using System.ComponentModel.DataAnnotations;
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
}