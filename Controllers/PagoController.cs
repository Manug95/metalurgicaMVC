using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Controllers;

public class PagoController(IPagoRepository repo): Controller
{
    private readonly IPagoRepository _repo = repo;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] FiltrosPago filters)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.MensajeError = Util.ModelStateViewError(ModelState);
            return View(new List<PagoVM>());
        }

        try
        {
            long cantidadPagos = await _repo.CountAsync(filters);
            List<Pago> pagos = cantidadPagos > 0 ? await _repo.ListAsync(filters) : [];

            decimal cp = Math.Ceiling((decimal)cantidadPagos / filters.Limit);

            ViewBag.cantPag = (int)(cp <= 0 ? 1 : cp);
            ViewBag.cantidadPaginado = filters.Limit;
            ViewBag.pagina = filters.Pagina;
            ViewBag.trabajoId = filters.TrabajoId;
            ViewBag.fecha = filters.Fecha;
            ViewBag.desde = filters.Desde;
            ViewBag.hasta = filters.Hasta;
            ViewBag.cliente = filters.Cliente;
            ViewBag.linkActivo = "pagos";
            ViewBag.MensajeError = TempData["MensajeError"] as string;

            return View(PagoVM.FromList(pagos));
        }
        catch (ClientException ex)
        {
            ViewBag.MensajeError = ex.Message;
        }
        catch (AppException)
        {
            ViewBag.MensajeError = "Error interno del servidor: No se pudieron traer los pagos";
        }

        return View(new List<PagoVM>());
    }
}