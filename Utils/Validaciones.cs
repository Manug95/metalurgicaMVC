using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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

public class ImageUrlListAttribute : ValidationAttribute
{
    private static readonly Regex _urlRegex = new Regex(
        @"^(http?:\/\/.*\.(?:png|jpg|jpeg|gif|bmp|webp))$", //cambiar luego a https
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IEnumerable urls)
        {
            foreach (var item in urls)
            {
                if (item is string url)
                {
                    if (!_urlRegex.IsMatch(url))
                    {
                        return new ValidationResult($"La URL '{url}' no es válida o no corresponde a una imagen.");
                    }
                }
                else
                {
                    return new ValidationResult("La colección contiene elementos que no son cadenas.");
                }
            }
        }
        return ValidationResult.Success!;
    }
}

public class FechaMayorQueAttribute : ValidationAttribute
{
    private readonly string _otraPropiedad;
    private readonly bool _igual;

    public FechaMayorQueAttribute(string otraPropiedad, bool igual)
    {
        _otraPropiedad = otraPropiedad;
        _igual = igual;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var propiedad = validationContext.ObjectType.GetProperty(_otraPropiedad);
        if (propiedad == null)
            return new ValidationResult($"La propiedad {_otraPropiedad} no existe.");

        var valorActual = value as DateTime?;
        var valorComparar = propiedad.GetValue(validationContext.ObjectInstance) as DateTime?;

        if (!valorActual.HasValue || !valorComparar.HasValue)
            return ValidationResult.Success;

        if (_igual)
        {
            if (valorActual.Value < valorComparar.Value)
                return new ValidationResult(ErrorMessage ?? $"La fecha debe ser mayor o igual que {_otraPropiedad}."); 
        }
        else
        {
            if (valorActual.Value <= valorComparar.Value)
                return new ValidationResult(ErrorMessage ?? $"La fecha debe ser mayor que {_otraPropiedad}.");
        }

        return ValidationResult.Success;
    }
}

public class FechaMayorOIgualQueHoyAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var valorActual = value as DateTime?;

        if (!valorActual.HasValue)
            return ValidationResult.Success;

        if (valorActual.Value < DateTime.Today)
            return new ValidationResult(ErrorMessage ?? $"La fecha no puede ser menor que la fecha actual.");

        return ValidationResult.Success;
    }
}

public class FechaMenorOIgualQueHoyAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var valorActual = value as DateTime?;

        if (!valorActual.HasValue)
            return ValidationResult.Success;

        if (valorActual.Value > DateTime.Today)
            return new ValidationResult(ErrorMessage ?? $"La fecha no puede ser mayor que la fecha actual.");

        return ValidationResult.Success;
    }
}