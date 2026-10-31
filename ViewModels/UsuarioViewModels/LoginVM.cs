using System.ComponentModel.DataAnnotations;

namespace metalurgicaMVC.ViewModels.UsuarioViewModels;

public class LoginVM
{
    [Required(ErrorMessage = "Es requerido")]
    [Display(Name = "Nombre de Usuario")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida2")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string? Password { get; set; }
    public string? ReturnUrl { get; set; }
}