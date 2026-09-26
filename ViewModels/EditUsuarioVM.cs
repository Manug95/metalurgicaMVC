using System.ComponentModel.DataAnnotations;

namespace metalurgicaMVC.ViewModels;

public class EditUsuarioVM
{
    public int Id { get; set; }
    public UpdatePasswordVM? UpdatePasswordVM { get; set; }

    [Display(Name = "Nombre de Usuario")]
    public string? Username { get; set; }

    [Display(Name = "ROL")]
    public string? Rol { get; set; }

    [Display(Name = "Foto Perfil")]
    public string? Avatar { get; set; }

    public IFormFile? AvatarFile { get; set; }
}