using System.ComponentModel.DataAnnotations;

namespace metalurgicaMVC.ViewModels.UsuarioViewModels;

public class UpdateDatosVM
{
    [Required(ErrorMessage = "El nombre de usuario no puede estar vacío")]
    [Display(Name = "Nombre de Usuario")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "El usuario necesita un rol")]
    [Display(Name = "ROL")]
    public string? Rol { get; set; }
}