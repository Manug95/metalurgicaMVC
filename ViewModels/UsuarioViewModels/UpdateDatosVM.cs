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

    [Required(ErrorMessage = "Falta el nombre")]
    [MaxLength(30, ErrorMessage = "Supera el máximo de 30 caracteres")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "Falta el apellido")]
    [MaxLength(30, ErrorMessage = "Supera el máximo de 30 caracteres")]
    public string? Apellido { get; set; }
}