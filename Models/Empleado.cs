using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Models;

public class Empleado
{
    [Key]
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Telefono { get; set; }

    public string? Seccion { get; set; }

    public decimal? Sueldo { get; set; }

    public DateTime? FechaContratado { get; set; }

    public DateTime? FechaFin { get; set; }

    public static Empleado From(EmpleadoVM vm)
    {
        Empleado empleado = new()
        {
            Id = vm.Id,
            Nombre = vm.Nombre,
            Apellido = vm.Apellido,
            Telefono = vm.Telefono,
            Seccion = vm.Seccion,
            Sueldo = vm.Sueldo,
            FechaContratado = vm.FechaContratado,
            FechaFin = vm.FechaFin
        };

        return empleado;
    }

    public static List<Empleado> FromList(List<EmpleadoVM> lista)
    {
        List<Empleado> empleados = [];

        foreach (EmpleadoVM c in lista)
        {
            empleados.Add(From(c));
        }

        return empleados;
    }

    public void CopyFrom(Empleado e)
    {
        Id = e.Id;
        Nombre = e.Nombre;
        Apellido = e.Apellido;
        Telefono = e.Telefono;
        Seccion = e.Seccion;
        Sueldo = e.Sueldo;
        FechaContratado = e.FechaContratado;
        FechaFin = e.FechaFin;
    }

    public void CopyFrom(EmpleadoVM vm)
    {
        Id = vm.Id;
        Nombre = vm.Nombre;
        Apellido = vm.Apellido;
        Telefono = vm.Telefono;
        Seccion = vm.Seccion;
        Sueldo = vm.Sueldo;
        FechaContratado = vm.FechaContratado;
        FechaFin = vm.FechaFin;
    }

    public void UpdateFrom(EmpleadoVM vm)
    {
        if (vm.Nombre != Nombre)
            Nombre = vm.Nombre;
        if (vm.Apellido != Apellido)
            Apellido = vm.Apellido;
        if (vm.Telefono != Telefono)
            Telefono = vm.Telefono;
        if (vm.Seccion != Seccion)
            Seccion = vm.Seccion;
        if (vm.Sueldo != Sueldo)
            Sueldo = vm.Sueldo;
        if (vm.FechaContratado != FechaContratado)
            FechaContratado = vm.FechaContratado;
        if (vm.FechaFin != FechaFin)
            FechaFin = vm.FechaFin;
    }
}