using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Models;

public class Cliente
{
    [Key]
    public int Id { get; set; }

    public string? Tipo { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Telefono { get; set; }

    public string? Cuit { get; set; }

    public string? Direccion { get; set; }

    public bool Activo { get; set; } = true;

    public override string ToString()
    {
        return $"{Apellido}, {Nombre}";
    }

    public static Cliente From(ClienteVM vm)
    {
        return new()
        {
            Id = vm.Id,
            Nombre = vm.Nombre,
            Apellido = vm.Apellido,
            Telefono = vm.Telefono,
            Cuit = vm.Cuit,
            Direccion = vm.Direccion,
            Tipo = vm.Tipo
        };
    }

    public static List<Cliente> FromList(List<ClienteVM> viewModels)
    {
        List<Cliente> clientes = [];
        foreach (var vm in viewModels)
        {
            clientes.Add(From(vm));
        }
        return clientes;
    }

    public void CopyValuesFrom(ClienteVM vm)
    {
        Nombre = vm.Nombre;
        Apellido = vm.Apellido;
        Telefono = vm.Telefono;
        Cuit = vm.Cuit;
        Direccion = vm.Direccion;
        Tipo = vm.Tipo;

        if (vm.Id > 0)
            Id = vm.Id;
    }

    public void CopyValuesFrom(Cliente c)
    {
        Nombre = c.Nombre;
        Apellido = c.Apellido;
        Telefono = c.Telefono;
        Cuit = c.Cuit;
        Direccion = c.Direccion;
        Tipo = c.Tipo;

        if (c.Id > 0)
            Id = c.Id;
    }
}