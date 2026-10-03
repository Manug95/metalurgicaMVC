using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;

namespace metalurgicaMVC.ViewModels;

public class TrabajoVM
{
    public long Id { get; set; }

    [Required(ErrorMessage = "La fecha de Solicitado es requerida")]
    [DataType(DataType.Date, ErrorMessage = "No es una fecha válida")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Fecha Solicitado")]
    public DateTime? FechaSolicitado { get; set; }

    // [Required(ErrorMessage = "La fecha de iniciado es requerida")]
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
    [MaxLength(512, ErrorMessage = "El detalle supera el máximo de 512 caracteres")]
    public string? Detalle { get; set; }

    [DataType(DataType.Text)]
    [MaxLength(512, ErrorMessage = "La observación supera el máximo de 512 caracteres")]
    public string? Observacion { get; set; }

    [Required(ErrorMessage = "El estado es requerido")]
    [EnumDataType(typeof(EstadoTrabajo), ErrorMessage = "Estado inválido")]
    public EstadoTrabajo? Estado { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad de días debe ser positiva")]
    public int DiasTrabajados { get; set; }

    // [Required]
    [Range(1, int.MaxValue, ErrorMessage = "ID cliente incorrecto")]
    public int ClienteId { get; set; }

    [Display(Name = "Cliente")]
    public Cliente? Cliente { get; set; }

    public static TrabajoVM From(Trabajo trabajo)
    {
        TrabajoVM dto = new()
        {
            Id = trabajo.Id,
            ClienteId = trabajo.ClienteId,
            FechaIniciado = trabajo.FechaIniciado,
            FechaSolicitado = trabajo.FechaSolicitado,
            FechaFinalizado = trabajo.FechaFinalizado,
            Importe = trabajo.Importe,
            Detalle = trabajo.Detalle,
            Observacion = trabajo.Observacion,
            DiasTrabajados = trabajo.DiasTrabajados,
            Estado = trabajo.Estado
        };

        return dto;
    }

    public static List<TrabajoVM> FromList(List<Trabajo> trabajos)
    {
        List<TrabajoVM> dtos = [];
        foreach (Trabajo t in trabajos)
        {
            dtos.Add(From(t));
        }
        return dtos;
    }
}