using System.ComponentModel.DataAnnotations;

namespace metalurgicaMVC.Utils;

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

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;
    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if (file != null)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_extensions.Contains(extension))
            {
                return new ValidationResult($"Extensión inválida. Solo se permiten: {string.Join(", ", _extensions)}");
            }
        }
        return ValidationResult.Success!;
    }
}