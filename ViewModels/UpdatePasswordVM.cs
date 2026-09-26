using System.ComponentModel.DataAnnotations;

namespace metalurgicaMVC.ViewModels;

public class UpdatePasswordVM
{
    [Required(ErrorMessage = "La contraseña actual es requerida")]
    [Display(Name = "Contraseña Actual")]
    public string? PasswordActual { get; set; }
    
    [Required(ErrorMessage = "La contraseña nueva es requerida")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    [Display(Name = "Contraseña Nueva")]
    public string? PasswordNuevo { get; set; }
}