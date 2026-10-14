using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels.UsuarioViewModels;

namespace metalurgicaMVC.ViewModels;

public class TrabajoVM
{
    public long Id { get; set; }

    [Required(ErrorMessage = "La fecha de Solicitado es requerida")]
    [DataType(DataType.Date, ErrorMessage = "No es una fecha válida")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Fecha Solicitado")]
    public DateTime? FechaSolicitado { get; set; }

    [DataType(DataType.Date, ErrorMessage = "No es una fecha válida")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [FechaMayorQue(nameof(FechaSolicitado), true, ErrorMessage = "La fecha iniciado debe ser mayor que la fecha solicitado.")]
    [Display(Name = "Fecha Iniciado")]
    public DateTime? FechaIniciado { get; set; }

    [DataType(DataType.Date, ErrorMessage = "No es una fecha válida")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [FechaMayorQue(nameof(FechaIniciado), true, ErrorMessage = "La fecha finalizado debe ser mayor que la fecha iniciado.")]
    [Display(Name = "Fecha Finalizado")]
    public DateTime? FechaFinalizado { get; set; }

    [DataType(DataType.Date, ErrorMessage = "No es una fecha válida")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [FechaMayorQue(nameof(FechaFinalizado), true, ErrorMessage = "La fecha retirado debe ser mayor que la fecha finalizado.")]
    [Display(Name = "Fecha Retirado")]
    public DateTime? FechaRetirado { get; set; }

    [Required(ErrorMessage = "El importe es requerido")]
    [DataType(DataType.Currency)]
    [Range(0, double.MaxValue, ErrorMessage = "El importe debe ser positivo")]
    public decimal? Importe { get; set; }

    [DataType(DataType.Text)]
    [MaxLength(512, ErrorMessage = "La descripción supera el máximo de 512 caracteres")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [DataType(DataType.Text)]
    [MaxLength(512, ErrorMessage = "La observación supera el máximo de 512 caracteres")]
    public string? Observacion { get; set; }

    [Required(ErrorMessage = "El estado es requerido")]
    [AllowedValues(["PRESUPUESTO", "ACEPTADO", "RECHAZADO"])]
    public string? Estado { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "ID cliente incorrecto")]
    public int ClienteId { get; set; }

    [Display(Name = "Cliente")]
    public ClienteVM? Cliente { get; set; }

    public int? AnuladorId { get; set; }

    public UsuarioVM? Anulador { get; set; }

    public int RegistradorId { get; set; }

    public UsuarioVM? Registrador { get; set; }

    public List<FotoVM>? Fotos { get; set; }
    public List<DetalleVM>? Detalles { get; set; }

    public static TrabajoVM From(Trabajo trabajo)
    {
        TrabajoVM vm = new()
        {
            Id = trabajo.Id,
            ClienteId = trabajo.ClienteId,
            FechaIniciado = trabajo.FechaIniciado,
            FechaSolicitado = trabajo.FechaSolicitado,
            FechaFinalizado = trabajo.FechaFinalizado,
            Importe = trabajo.Importe,
            Observacion = trabajo.Observacion,
            Estado = trabajo.Estado
        };

        if (trabajo.Fotos != null)
            vm.Fotos = FotoVM.FromList(trabajo.Fotos);
        if (trabajo.Detalles != null)
            vm.Detalles = DetalleVM.FromList(trabajo.Detalles);
        if (trabajo.Anulador != null)
            vm.Anulador = UsuarioVM.From(trabajo.Anulador);
        if (trabajo.Registrador != null)
            vm.Registrador = UsuarioVM.From(trabajo.Registrador);
        if (trabajo.Cliente != null)
            vm.Cliente = ClienteVM.From(trabajo.Cliente);

        return vm;
    }

    public static List<TrabajoVM> FromList(List<Trabajo> trabajos)
    {
        List<TrabajoVM> viewModels = [];
        foreach (Trabajo t in trabajos)
        {
            viewModels.Add(From(t));
        }
        return viewModels;
    }
}