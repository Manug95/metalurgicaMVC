using System.ComponentModel.DataAnnotations;
using metalurgicaMVC.ViewModels;

namespace metalurgicaMVC.Models;

public class Foto
{
    [Key]
    public long Id { get; set; }
    public string? Ruta { get; set; }
    public long TrabajoId { get; set; }

    public static Foto From(FotoVM vm)
    {
        return new()
        {
            Id = vm.Id,
            Ruta = vm.Ruta,
            TrabajoId = vm.TrabajoId
        };
    }

    public static List<Foto> FromList(List<FotoVM> viewModels)
    {
        List<Foto> fotos = [];
        foreach (var vm in viewModels)
        {
            fotos.Add(From(vm));
        }
        return fotos;
    }
}