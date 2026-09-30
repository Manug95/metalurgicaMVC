using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Controllers;

public class EmpleadoController(IEmpleadoRepository repo) : Controller
{
    private readonly IEmpleadoRepository _repo = repo;
    
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] FiltrosEmpleado filters)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.MensajeError = Util.ModelStateViewError(ModelState);
            return View(new List<EmpleadoVM>());
        }

        try
        {
            List<Empleado> empleados = await _repo.ListAsync(filters);
            int cantidadEmpleados = await _repo.CountAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadEmpleados / filters.Limit);

            ViewBag.cantPag = (int)(cp <= 0 ? 1 : cp);
            ViewBag.cantidadPaginado = filters.Limit;
            ViewBag.pagina = filters.Pagina;
            ViewBag.busqueda = filters.Busqueda;
            ViewBag.linkActivo = "empleados";
            ViewBag.MensajeError = TempData["MensajeError"] as string;

            return View(EmpleadoVM.FromList(empleados));
        }
        catch (ClientException ex)
        {
            ViewBag.MensajeError = ex.Message;
        }
        catch (AppException)
        {
            ViewBag.MensajeError = "Error interno del servidor: No se pudieron traer los empleados";
        }

        return View(new List<Empleado>());
    }
}