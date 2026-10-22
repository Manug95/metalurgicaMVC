using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Controllers;

public class MaterialController(IMaterialRepository materialRepository) : Controller
{
    private readonly IMaterialRepository _repo = materialRepository;

    [HttpGet]
    public async Task<IActionResult> Index(FiltrosMaterial filters)
    {
        List<Material> materiales = [];

        if (!ModelState.IsValid)
        {
            ViewBag.MensajeError = Util.ModelStateViewError(ModelState);
            return View(MaterialVM.FromList(materiales));
        }

        try
        {
            int cantidadMateriales = await _repo.CountAsync(filters);
            if (cantidadMateriales > 0)
                materiales = await _repo.ListAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadMateriales / filters.Limit);

            ViewBag.cantPag = (int)(cp <= 0 ? 1 : cp);
            ViewBag.cantidadPaginado = filters.Limit;
            ViewBag.pagina = filters.Pagina;
            ViewBag.tipoMaterial = filters.Tipo;
            ViewBag.linkActivo = "materiales";
            ViewBag.MensajeError = TempData["MensajeError"] as string;

            return View(MaterialVM.FromList(materiales));
        }
        catch (ClientException ex)
        {
            ViewBag.MensajeError = ex.Message;
        }
        catch (AppException)
        {
            ViewBag.MensajeError = "Error interno del servidor: No se pudieron traer los materiales";
        }

        return View(MaterialVM.FromList(materiales));
    }
}