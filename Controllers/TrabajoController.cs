using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Controllers;

public class TrabajoController(ITrabajoRepository repo) : Controller
{
    private readonly ITrabajoRepository _repo = repo;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] FiltrosTrabajo filters)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.MensajeError = Util.ModelStateViewError(ModelState);
            return View(new List<TrabajoVM>());
        }

        try
        {
            List<Trabajo> trabajos = await _repo.ListAsync(filters);
            long cantidadTrabajos = await _repo.CountAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadTrabajos / filters.Limit);

            ViewBag.cantPag = (int)(cp <= 0 ? 1 : cp);
            ViewBag.cantidadPaginado = filters.Limit;
            ViewBag.pagina = filters.Pagina;
            ViewBag.busqueda = "";
            ViewBag.linkActivo = "trabajos";
            ViewBag.MensajeError = TempData["MensajeError"] as string;

            return View(TrabajoVM.FromList(trabajos));
        }
        catch (ClientException ex)
        {
            ViewBag.MensajeError = ex.Message;
        }
        catch (AppException)
        {
            ViewBag.MensajeError = "Error interno del servidor: No se pudieron traer los trabajos";
        }

        return View(new List<TrabajoVM>());
    }
    
    public IActionResult Create()
    {
        return View();
    }
    
    public IActionResult Edit([FromRoute] long id)
    {
        return View("Create");
    }
}