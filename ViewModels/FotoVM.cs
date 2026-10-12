using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.Models;

namespace metalurgicaMVC.ViewModels;

public class FotoVM
{
    [Range(0, long.MaxValue, ErrorMessage = "ID de la foto es incorrecta")]
    public long Id { get; set; }

    public string? Ruta { get; set; }

    public List<Foto> Fotos { get; set; } = [];

    [Required(ErrorMessage = "La ID del trabajo es requerida")]
    [Range(1, long.MaxValue, ErrorMessage = "La ID del trabajo es incorrecta")]
    public long TrabajoId { get; set; }

    [Required(ErrorMessage = "No se subieron archivos")]
    [Display(Name = "Subir Foto(s)")]
    public List<IFormFile>? FotosFiles { get; set; }

    public static FotoVM From(Foto vm)
    {
        return new()
        {
            Id = vm.Id,
            Ruta = vm.Ruta,
            TrabajoId = vm.TrabajoId
        };
    }

    public static List<FotoVM> FromList(List<Foto> fotos)
    {
        List<FotoVM> lista = [];
        foreach (var f in fotos)
        {
            lista.Add(From(f));
        }
        return lista;
    }
}