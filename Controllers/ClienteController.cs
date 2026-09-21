using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace metalurgicaMVC.Controllers;

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

        try
        {
            List<Cliente> clientes = await _repo.ListAsync(filters);
            int cantidadClientes = await _repo.CountAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadClientes / filters.Limit);

            ViewBag.cantPag = (int)(cp <= 0 ? 1 : cp);
            ViewBag.cantidadPaginado = filters.Limit;
            ViewBag.pagina = filters.Pagina;
            ViewBag.busqueda = filters.Busqueda;
            ViewBag.linkActivo = "clientes";
            ViewBag.MensajeError = TempData["MensajeError"] as string;

            return View(ClienteVM.ParseList(clientes));
        }
        catch (ClientException ex)
        {
            ViewBag.MensajeError = ex.Message;
        }
        catch (AppException)
        {
            ViewBag.MensajeError = "Error interno del servidor: No se pudieron traer los clientes";
        }

        return View(new List<Cliente>());
    }

    [HttpGet]
    public async Task<IActionResult> Form()
    {
        return PartialView("_FormModal", new ClienteVM());
    }
}