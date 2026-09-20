using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Models;

public class Cliente
{
    [Key]
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Telefono { get; set; }

    public string? Cuit { get; set; }

    public string? Direccion { get; set; }

    public bool Activo { get; set; } = true;

    public static Cliente Parse(ClienteVM dto)
    {
        return new()
        {
            Id = dto.Id,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Telefono = dto.Telefono,
            Cuit = dto.Cuit,
            Direccion = dto.Direccion
        };
    }

    public void CopyValuesFrom(ClienteVM dto)
    {
        Nombre = dto.Nombre;
        Apellido = dto.Apellido;
        Telefono = dto.Telefono;
        Cuit = dto.Cuit;
        Direccion = dto.Direccion;

        if (dto.Id > 0)
            Id = dto.Id;
    }

    public void CopyValuesFrom(Cliente c)
    {
        Nombre = c.Nombre;
        Apellido = c.Apellido;
        Telefono = c.Telefono;
        Cuit = c.Cuit;
        Direccion = c.Direccion;

        if (c.Id > 0)
            Id = c.Id;
    }
}