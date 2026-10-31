using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Controllers;

[Authorize]
public class ClienteController(IClienteRepository repo) : Controller
{
    private readonly IClienteRepository _repo = repo;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] FiltrosCliente filters)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.MensajeError = Util.ModelStateViewError(ModelState);
            return View(new List<ClienteVM>());
        }

        List<Cliente> clientes = [];
        try
        {
            int cantidadClientes = await _repo.CountAsync(filters);
            if (cantidadClientes > 0)
                clientes = await _repo.ListAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadClientes / filters.Limit);

            ViewBag.cantPag = (int)(cp <= 0 ? 1 : cp);
            ViewBag.cantidadPaginado = filters.Limit;
            ViewBag.pagina = filters.Pagina;
            ViewBag.busqueda = filters.Busqueda;
            ViewBag.linkActivo = "clientes";
            ViewBag.MensajeError = TempData["MensajeError"] as string;

            return View(ClienteVM.FromList(clientes));
        }
        catch (ClientException ex)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return View("BadRequest", ex.Message);
        }
        catch (AppException ex)
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return View("InternalServerError", $"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }
}