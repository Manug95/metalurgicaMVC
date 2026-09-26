using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace metalurgicaMVC.ViewModels.UsuarioViewModels;

public enum EstadoUsuario
{
    INACTIVO = 0,
    ACTIVO = 1,
}

public class FiltrosUsuario : Paginado
{
    [MaxLength(60, ErrorMessage = "La búsqueda acepta 60 caracteres máximo")]
    public string? Username { get; set; }

    // [AllowNull]
    // [AllowedValues(["ADMIN", "EMPLEADO"], ErrorMessage = "El rol debe ser ADMIN o EMPLEADO")]
    public string? Rol { get; set; }

    [EnumDataType(typeof(EstadoUsuario), ErrorMessage = "estado inválido")]
    public EstadoUsuario? Estado { get; set; }
}