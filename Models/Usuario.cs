using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Models;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    public string? Username { get; set; }

    [Column("pwd")]
    public string? Password { get; set; }

    public string? Rol { get; set; }

    public string? Avatar { get; set; }

    public bool Activo { get; set; } = true;

    public static Usuario From(UsuarioVM vm)
    {
        return new()
        {
            Id = vm.Id,
            Username = vm.Username,
            Password = vm.Password,
            Rol = vm.Rol,
            Avatar = vm.Avatar
        };
    }

    public static List<Usuario> FromList(List<UsuarioVM> lista)
    {
        List<Usuario> usuarios = [];
        foreach (var vm in lista)
        {
            usuarios.Add(From(vm));
        }
        return usuarios;
    }

    public void CopyFrom(Usuario u)
    {
        Id = u.Id;
        Username = u.Username;
        Password = u.Password;
        Rol = u.Rol;
        Avatar = u.Avatar;
        Activo = u.Activo;
    }

    public void CopyFrom(UsuarioVM vm)
    {
        Id = vm.Id;
        Username = vm.Username;
        Rol = vm.Rol;
        Avatar = vm.Avatar;
    }
}