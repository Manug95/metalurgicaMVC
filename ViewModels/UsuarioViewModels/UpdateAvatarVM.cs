using System.ComponentModel.DataAnnotations;

namespace metalurgicaMVC.ViewModels.UsuarioViewModels;

public class UpdateAvatarVM
{
    [Display(Name = "Foto Perfil")]
    public string? Avatar { get; set; }

    [Required(ErrorMessage = "No se envió ningun archivo")]
    [FileExtensions(ErrorMessage = "El archivo tiene una extensión inválida", Extensions = "jpg,jpeg,webp,png,gif")]
    [MaxFileSize(5)] // máximo 2 MB
    [DataType(DataType.Upload)]
    public IFormFile? AvatarFile { get; set; }
}

public class MaxFileSizeAttribute(int maxSizeInMb) : ValidationAttribute
{
    private readonly int _maxSizeInMb = maxSizeInMb;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if (file != null && file.Length > _maxSizeInMb * 1024 * 1024)
        {
            return new ValidationResult($"El archivo no puede superar los {_maxSizeInMb} MB.");
        }
        return ValidationResult.Success;
    }
}