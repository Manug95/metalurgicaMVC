using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.Utils;

namespace metalurgicaMVC.ViewModels.UsuarioViewModels;

public class UpdateAvatarVM
{
    public int Id { get; set; }
    
    [Display(Name = "Foto Perfil")]
    public string? Avatar { get; set; }

    [Required(ErrorMessage = "No se envió ningun archivo")]
    [AllowedExtensions([".jpg", ".jpeg", ".webp", ".png", ".gif"])]
    [MaxFileSize(5)] // máximo 2 MB
    [DataType(DataType.Upload)]
    public IFormFile? AvatarFile { get; set; }
}