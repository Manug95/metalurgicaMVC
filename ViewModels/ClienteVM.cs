using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.Models;

namespace metalurgicaMVC.ViewModels;

public class ClienteVM
{
    public int Id { get; set; }

    [MaxLength(100, ErrorMessage = "El máximo de caracteres es 100")]
    [Required(ErrorMessage="El nombre es requerido")]
    public string? Nombre { get; set; }

    [MaxLength(100, ErrorMessage = "El máximo de caracteres es 100")]
    [Required(ErrorMessage = "El apellido es requerido")]
    public string? Apellido { get; set; }

    [RegularExpression(@"^\+?[0-9\s\-]{6,20}$", ErrorMessage = "El formato teléfono es inválido")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [RegularExpression(@"^\d{2}\-\d{7,8}\-\d{1}$", ErrorMessage = "El formato CUIT es inválido")]
    [Display(Name = "C.U.I.T")]
    public string? Cuit { get; set; }

    [StringLength(100, ErrorMessage = "El maximo de caracteres es 100")]
    [Display(Name = "Dirección")]
    public string? Direccion { get; set; }

    public static ClienteVM Parse(Cliente cliente)
    {
        ClienteVM dto = new()
        {
            Id = cliente.Id,
            Nombre = cliente.Nombre,
            Apellido = cliente.Apellido,
            Telefono = cliente.Telefono,
            Cuit = cliente.Cuit,
            Direccion = cliente.Direccion
        };

        return dto;
    }

    public static List<ClienteVM> ParseList(List<Cliente> clientes)
    {
        List<ClienteVM> dtos = [];

        foreach (Cliente c in clientes)
        {
            dtos.Add(Parse(c));
        }

        return dtos;
    }
}