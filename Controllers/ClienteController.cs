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
        }
        catch (ClientException ex)
        {
            ViewBag.MensajeError = ex.Message;
        }
        catch (AppException)
        {
            ViewBag.MensajeError = "Error interno del servidor: No se pudieron traer los clientes";
        }

        return View(ClienteVM.FromList(clientes));
    }
}