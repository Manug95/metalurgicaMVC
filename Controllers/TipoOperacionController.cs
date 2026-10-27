using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Controllers;

public  class TipoOperacionController(ITipoOperacionRepository tipoOperacionRepository) : Controller
{
    private readonly ITipoOperacionRepository _repo = tipoOperacionRepository;

    public async Task<IActionResult> Index([FromQuery] FiltrosTipoOperacion filters)
    {
        List<TipoOperacion> tiposOperacion = [];

        if (!ModelState.IsValid)
        {
            ViewBag.MensajeError = Util.ModelStateViewError(ModelState);
            return View(TipoOperacionVM.FromList(tiposOperacion));
        }

        try
        {
            int cantidadTiposOperacion = await _repo.CountAsync(filters);
            if (cantidadTiposOperacion > 0)
                tiposOperacion = await _repo.ListAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadTiposOperacion / filters.Limit);

            ViewBag.cantPag = (int)(cp <= 0 ? 1 : cp);
            ViewBag.cantidadPaginado = filters.Limit;
            ViewBag.pagina = filters.Pagina;
            ViewBag.linkActivo = "tipoOperacion";
            ViewBag.MensajeError = TempData["MensajeError"] as string;
        }
        catch (ClientException ex)
        {
            ViewBag.MensajeError = ex.Message;
        }
        catch (AppException)
        {
            ViewBag.MensajeError = "Error interno del servidor: No se pudieron traer los tipos de operaciones";
        }

        return View(TipoOperacionVM.FromList(tiposOperacion));
    }
}