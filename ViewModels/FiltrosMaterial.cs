using System.ComponentModel.DataAnnotations;

namespace metalurgicaMVC.ViewModels;

public class FiltrosMaterial : Paginado
{
    // [AllowedValues(["", "CHAPA", "CAÑO", "PINTURA"], ErrorMessage = "Tipo incorrecto")]
    public string? Tipo { get; set; }
}