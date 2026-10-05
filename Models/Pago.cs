using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace metalurgicaMVC.Models;

public class Pago
{
    [Key]
    public long Id { get; set; }

    public long TrabajoId { get; set; }

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
}