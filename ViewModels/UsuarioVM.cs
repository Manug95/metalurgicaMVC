using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.Models;

namespace metalurgicaMVC.ViewModels;

public class UsuarioVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es requerido")]
    [StringLength(20, ErrorMessage = "El nombre de ususario debe tener entre 3 y 20 caracteres", MinimumLength = 3)]
    public string? Username { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "El rol es requerido")]
    [AllowedValues(["ADMIN", "EMPLEADO"], ErrorMessage = "El rol debe ser ADMIN o EMPLEADO")]
    public string? Rol { get; set; }

    [MaxLength(255, ErrorMessage = "La URL del avatar es muy larga")]
    [DataType(DataType.ImageUrl, ErrorMessage = "No es una URL de imagen")]
    public string? Avatar { get; set; }

    public bool Activo { get; set; }

    public static UsuarioVM From(Usuario u)
    {
        return new()
        {
            Id = u.Id,
            Username = u.Username,
            Rol = u.Rol,
            Avatar = u.Avatar,
            Activo = u.Activo
        };
    }

    public static List<UsuarioVM> FromList(List<Usuario> usuarios)
    {
        List<UsuarioVM> lista = [];
        foreach (var u in usuarios)
        {
            lista.Add(From(u));
        }
        return lista;
    }
}