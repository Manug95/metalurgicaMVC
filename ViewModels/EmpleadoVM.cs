using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.Models;

namespace metalurgicaMVC.ViewModels;

public class EmpleadoVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(60, ErrorMessage = "El máximo de caracteres es 60")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "El apellido es requerido")]
    [MaxLength(60, ErrorMessage = "El máximo de caracteres es 60")]
    public string? Apellido { get; set; }

    [Required(ErrorMessage = "El Teléfono es requerido")]
    [RegularExpression(@"^\+?[0-9\s\-]{6,20}$", ErrorMessage = "El formato teléfono es inválido")]
    [MaxLength(20, ErrorMessage = "El máximo de caracteres es 20")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "La sección es requerido")]
    [AllowedValues(["OFICINA", "TALLER"], ErrorMessage = "La sección debe ser OFICINA o TALLER")]
    [Display(Name = "Sección")]
    public string? Seccion { get; set; }

    [Required(ErrorMessage = "El sueldo es requerido")]
    [DataType(DataType.Currency)]
    public decimal? Sueldo { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha Contratado")]
    public DateTime? FechaContratado { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha Fin")]
    public DateTime? FechaFin { get; set; }

    public bool Activo { get; set; }

    public static EmpleadoVM From(Empleado empleado)
    {
        EmpleadoVM vm = new()
        {
            Id = empleado.Id,
            Nombre = empleado.Nombre,
            Apellido = empleado.Apellido,
            Telefono = empleado.Telefono,
            Seccion = empleado.Seccion,
            Sueldo = empleado.Sueldo,
            FechaContratado = empleado.FechaContratado,
            FechaFin = empleado.FechaFin,
            Activo = empleado.Activo
        };

        return vm;
    }

    public static List<EmpleadoVM> FromList(List<Empleado> empleados)
    {
        List<EmpleadoVM> lista = [];

        foreach (Empleado c in empleados)
        {
            lista.Add(From(c));
        }

        return lista;
    }
}